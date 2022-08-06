using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyStipendCalcAPI.Models;
using StudyStipendCalcAPI.Repositories;
using StudyStipendCalcAPI.Services.StudentServices;
using System.Linq;
using System.Collections.Generic;
using StudyStipendCalcAPI.DTOs;
using StudyStipendCalcAPI.Services.StudentServices.CalculatorServices;
using StudyStipendCalcAPI.DTOs.FileDtos;
using StudyStipendCalcAPI.DTOs.StudentDtos;

namespace StudyStipendCalcAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studrepo;
        private readonly IUniversityRepository _unirepo;
        private readonly IRoleRepository _rolerepo;

        public StudentController(IMapper mapper, IStudentRepository studrepo, IUniversityRepository unirepo, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _studrepo = studrepo;
            _unirepo = unirepo;
            _rolerepo = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var data = await _studrepo.GetAllStud();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]UploadDto fileInput)
        {
            try
            {
                string name = fileInput.file.FileName;
                var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources",fileInput.uni);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var stream = new FileStream(Path.Combine(path,name), FileMode.Create);
                await fileInput.file.CopyToAsync(stream);
                if (await _unirepo.UniExist(Int32.Parse(fileInput.uni)))
                {
                    List<StudentDto> stud = ReadExcel.ReadTanulmanyiExcelFile(Int32.Parse(fileInput.uni), Path.Combine(path,name));
                    foreach (var item in stud)
                    {
                        var StudentToCreate = _mapper.Map<Student>(item);
                        var createdStudent = await _studrepo.AddToDB(StudentToCreate);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("group")]
        public IActionResult GetStudentsInGroup(GroupStudentInputDto input)
        {
            Dictionary<string, List<StudentDto>> grouppedstudentDtos = new Dictionary<string, List<StudentDto>>();
            SemesterType sem = input.Semester == "2" ? SemesterType.Spring : SemesterType.Autumn;
            foreach (var item in _studrepo.GetGrouppedStudents(sem, input.Faculty))
            {
                grouppedstudentDtos.Add(item.Key, new List<StudentDto>());
                foreach (var stud in item.Value)
                {
                    var studDto = _mapper.Map<StudentDto>(stud);
                    grouppedstudentDtos.GetValueOrDefault(item.Key).Add(studDto);
                }
            }
            // visszadjuk a csoportokat megfelelően csoportosítva és ezt megjelenítjük frontenden
            return Ok(grouppedstudentDtos);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateStudent(StudentDto student)
        {
            try
            {
                ReadExcel.CalculateStudentFields(student);
                var stud = _mapper.Map<StudentDto, Student>(student);
                await _studrepo.ModifyStudent(stud);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
           
            //GenerateWord.OpenWordTemplateInsertData(new System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<DTOs.UserDTOs.StudentDto>>(), DTOs.UserDTOs.SemesterType.Spring, "asd", "ÖM");
                return Ok();
            

        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> SearchStudent(FilterStudentDto userDto)
        {
            var data = await _studrepo.FilterStudent(
                int.Parse(userDto.roleid),
                int.Parse(userDto.uniid),
                _rolerepo.UniRoleToFaculty);
            return Ok(data);
        }

    }
}