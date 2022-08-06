using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyStipendCalcAPI.Models;
using StudyStipendCalcAPI.Repositories;
using Microsoft.AspNetCore.Http;
using StudyStipendCalcAPI.DTOs;

namespace StudyStipendCalcAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UniController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUniversityRepository _repo;

        public UniController(IMapper mapper, IUniversityRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUni()
        {
            var data = await _repo.GetAllUniFromDB();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddUni(UniversitiesDto uni)
        {
            try
            {
                var createUni = _mapper.Map<Universities>(uni);
                await _repo.AddToDB(createUni);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUni(int id)
        {
            bool res;
            try
            {
                res = await _repo.DeleteUni(id);
                return Ok(res);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
