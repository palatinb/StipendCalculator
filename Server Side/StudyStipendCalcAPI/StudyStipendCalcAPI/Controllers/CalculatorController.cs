using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyStipendCalcAPI.DTOs;
using StudyStipendCalcAPI.DTOs.StudentDtos;
using StudyStipendCalcAPI.Helpers;
using StudyStipendCalcAPI.Models;
using StudyStipendCalcAPI.Repositories;
using StudyStipendCalcAPI.Services.StudentServices.CalculatorServices;

namespace StudyStipendCalcAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studrepo;
        private readonly IRoleRepository _rolerepo;

        public CalculatorController(IMapper mapper, IStudentRepository studrepo,IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _studrepo = studrepo;
            _rolerepo = roleRepository;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate(CalculateDto dto)
        {
            var res = await Task.Run(() => ScholarshipCalculator.Calculator(dto.students, dto.input, dto.minStipendIndex, dto.minPrice, dto.maxPrice));
            return Ok(res);
        }

        [HttpPut("save")]
        public async Task<IActionResult> SaveChanges(List<StudentDto> students)
        {
            try
            {
                foreach (var item in students)
                {
                    var stud = _mapper.Map<Student>(item);
                    await _studrepo.ModifyStudent(stud);
                }
                return Ok();
            }
            catch
            {
                return Conflict();
            }
        }

        [HttpPost("faculty")]
        public IActionResult getAllowedFaculty(FilterStudentDto studdto)
        {
                var data = _rolerepo.UniRoleToFaculty.SingleOrDefault(p => p.Key == int.Parse(studdto.uniid)).Value.SingleOrDefault(q => q.Key == int.Parse(studdto.roleid)).Value;
            return Ok(data);
        }
        [HttpPost("semester")]
        public IActionResult changeDb(FilterStudentDto studdto)
        {
                var data = _rolerepo.UniRoleToFaculty.SingleOrDefault(p => p.Key == int.Parse(studdto.uniid)).Value.SingleOrDefault(q => q.Key == int.Parse(studdto.roleid)).Value;
            return Ok(data);
        }
    }
}
