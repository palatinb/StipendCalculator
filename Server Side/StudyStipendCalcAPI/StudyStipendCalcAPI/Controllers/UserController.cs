using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyStipendCalcAPI.DTOs;
using StudyStipendCalcAPI.DTOs.UserDTOs;
using StudyStipendCalcAPI.Models;
using StudyStipendCalcAPI.Repositories;

namespace StudyStipendCalcAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IRoleRepository _rolerepo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IConfiguration config, IMapper mapper, IRoleRepository rolerepo)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
            _rolerepo = rolerepo;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool res;
            try
            {
                res = await _repo.DeleteUser(id);
                return Ok(res);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ModifyUser(UserModifyDto modifieduser)
        {
            if (!await _repo.UserExists(modifieduser.id))
                return BadRequest("User nem létezik");
            var userEdited = _mapper.Map<User>(modifieduser);
            if (await _repo.EditUser(userEdited, modifieduser.PasswordHash))
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [Authorize]
        [HttpPut]
        [Route("bya")]
        public async Task<IActionResult> ModifyUserbyAdmin(UserModifyDto modifieduser)
        {
            if (!await _repo.UserExists(modifieduser.id))
                return BadRequest("User nem létezik");
            var userEdited = _mapper.Map<User>(modifieduser);
            if (await _repo.EditUserByAdmin(userEdited, modifieduser.PasswordHash))
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await _repo.GetAllUser();
            List<UserResponseDto> resp = new List<UserResponseDto>();
            foreach (var item in data)
            {
                var respUser = _mapper.Map<UserResponseDto>(item);
                respUser.RoleName = await _rolerepo.GetRoleName(item.RoleId);
                resp.Add(respUser);
            }
            return Ok(resp);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user =await _repo.GetUserById(id);
            var usershow = _mapper.Map<UserModifyDto>(user);
            usershow.PasswordHash = null;
            return Ok(usershow);

        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerDto)
        {
            registerDto.Username = registerDto.Username.ToLower();
            if (await _repo.UsernameExist(registerDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(registerDto);
            var createdUser = await _repo.Register(userToCreate, registerDto.PasswordHash);
            return StatusCode(201, new { username = createdUser.Username, name = createdUser.Name });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            var userFromRepo = await _repo.Login(loginDto.Username.ToLower(), loginDto.Password,loginDto.Last_login);
            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim("id", userFromRepo.Id.ToString()),
                new Claim("username", userFromRepo.Username),
                new Claim("roleid", userFromRepo.RoleId.ToString()),
                new Claim("name", userFromRepo.Name),
                new Claim("uniid", userFromRepo.UiD.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
           
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [Authorize]
        [HttpPost]
        [Route("check")]
        public async Task<IActionResult> CheckOldPassw(OldPasswDto dto)
        {
            var res = await _repo.CheckOldPassw(dto.id, dto.passw);
            return Ok(res);
        }

        [Authorize]
        [HttpPost]
        [Route("checkuser")]
        public async Task<IActionResult> CheckOldUsername(RegisterUserDto register)
        {
            var res = await _repo.UsernameExist(register.Username);
            return Ok(res);
        }
    }
}