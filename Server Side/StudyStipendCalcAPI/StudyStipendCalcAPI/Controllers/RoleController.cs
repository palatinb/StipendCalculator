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
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repo;

        public RoleController(IRoleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRole()
        {
            var data = await _repo.GetAllRoles();
            return Ok(data);
        }
    }
}