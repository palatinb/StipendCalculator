using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyStipendCalcAPI.DTOs.FileDtos;
using StudyStipendCalcAPI.DTOs.StudentDtos;
using StudyStipendCalcAPI.Repositories;
using StudyStipendCalcAPI.Services.FileServices;
using StudyStipendCalcAPI.Services.StudentServices;

namespace StudyStipendCalcAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratorController : ControllerBase
    {
        private readonly IStudentRepository _studRepo;
        private readonly IMapper _mapper;

        public GeneratorController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studRepo = studentRepository;
            _mapper = mapper;
            //SemesterType sem = input.Semester == "2" ? SemesterType.Spring : SemesterType.Autumn;
        }

        //[HttpPost]
        //public async Task<IActionResult> Test(GenerateDto dto)
        //{
        //    var rolename = await _roleRepository.GetRoleName(Convert.ToInt32(dto.Roleid));
        //    dto.facultyName = GetFacultyName(dto.faculty);
        //    SemesterType sem = dto.SemesterType == "2" ? SemesterType.Spring : SemesterType.Autumn;
        //    dto.studentsInGroup = _studRepo.GetGrouppedStudents(sem, dto.faculty);
        //    Dictionary<string, List<Student>> studentsInGroup = _studRepo.GetGrouppedStudents(DTOs.StudentDtos.SemesterType.Autumn, "N");
        //    var year = sem == SemesterType.Autumn ? $"{DateTime.Now.Year}/{DateTime.Now.AddYears(1).Year.ToString().Remove(0, 2)}/1" : $"{DateTime.Now.AddYears(-1).Year}/{DateTime.Now.Year.ToString().Remove(0, 2)}/2";
        //    dto.studentsInGroupList = new List<Student>();
        //    dto.MonthName = GetMonthName(dto.Month);
        //    foreach (var item in dto.studentsInGroup)
        //    {
        //        foreach (var stud in item.Value)
        //        {

        //            dto.studentsInGroupList.Add(stud);
        //        }
        //    }

        //    GenerateWord.GenerateElnökiHatározatWord(dto, rolename);
        //    GenerateWord.GenerateBizonylat(dto, rolename);
        //    //GenerateWord.GenerateTanulmanyiÖsszesito_Word(rolename, dto);
        //    GenerateWord.GenerateUtalas(dto, rolename);
        //    GenerateWord.GenerateÖCSI_Word(dto, rolename, dto.studentsInGroupList, year);
        //    foreach (var item in dto.studentsInGroup)
        //    {

        //        GenerateWord.GenerateÖsztöndijMutató_Word(dto, item, rolename);
        //    }
        //    GenerateExcel.GenerateNeptunba_Excel(dto);
        //    return Ok();
        //}

        [HttpGet("newfolder")]
        public IActionResult GenerateTemplateFolder()
        {
            try
            {
                string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources", "1", "Templates");
                var res = Directory.Exists(path);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// DONE
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("ocsi")]
        public IActionResult GenerateÖCSI(GenerateDto dto)
        {
            try
            {
                //var rolename = await _roleRepository.GetRoleName(Convert.ToInt32(dto.Roleid));
                SemesterType sem = dto.SemesterType == "2" ? SemesterType.Spring : SemesterType.Autumn;

                dto.studentsInGroup = GetStudentsInGroup(sem, dto.faculty);

                dto.facultyName = GetFacultyName(dto.faculty);

                var year =
                    sem == SemesterType.Autumn ?
                        $"{DateTime.Now.Year}/{DateTime.Now.AddYears(1).Year.ToString().Remove(0, 2)}/1" :
                        $"{DateTime.Now.AddYears(-1).Year}/{DateTime.Now.Year.ToString().Remove(0, 2)}/2";

                dto.studentsInGroupList = ConvertDictionaryToList(dto.studentsInGroup);

                string öcsiWordPath = FileGeneratorFactory.GenerateÖCSI_Word(dto, dto.studentsInGroupList.OrderByDescending(q => q.GroupIndex).ToList(), year);

                return DownloadFileAsync(öcsiWordPath).Result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("elnok")]
        public async Task<IActionResult> GenerateElnökiHat(GenerateDto dto)
        {
            try
            {
                dto.facultyName = GetFacultyName(dto.faculty);
                List<string> resStrings = new List<string>();
                ZipFile zip = new ZipFile();

                dto.MonthName = GetMonthName(dto.Month);
                var res = FileGeneratorFactory.GenerateElnökiHatározatWord(dto);

                var memory = new MemoryStream();
                using (var stream = new FileStream(res, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                return File(memory, GetMimeType(res), res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("mutato")]
        [Obsolete]
        public IActionResult GenerateMutatok(GenerateDto dto)
        {
            try
            {
                //var rolename = await _roleRepository.GetRoleName(Convert.ToInt32(dto.Roleid));
                //List<Student> students = _studRepo.FilterStudent(Convert.ToInt32(dto.Roleid), Convert.ToInt32(dto.Uniid), _roleRepository.UniRoleToFaculty).Result;
                //List<Student> filtereedStudnets = students.Where(q => q.FinancialState.Contains("Állami")).ToList();
                //var res = GenerateWord.GenerateElnökiHatározatWord(dto, rolename);
                SemesterType sem = dto.SemesterType == "2" ? SemesterType.Spring : SemesterType.Autumn;
                dto.studentsInGroup = GetStudentsInGroup(sem, dto.faculty);

                dto.facultyName = GetFacultyName(dto.faculty);

                List<string> resStrings = new List<string>();
                foreach (var item in dto.studentsInGroup)
                {
                    resStrings.Add(FileGeneratorFactory.GenerateÖsztöndijMutató_Word(dto, item));
                }

                ZipFile zip = new ZipFile();

                zip.UseUnicodeAsNecessary = true;
                zip.AddFiles(resStrings, "./");
                string savedirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources", dto.Uniid, dto.Roleid);
                string savepath = Path.Combine(savedirectory, $"{dto.facultyName} mutatók.zip");

                zip.Save(savepath);

                return DownloadFileAsync(savepath).Result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("tanulmanyi")]
        [Obsolete]
        public IActionResult GenerateTanulmanyi([FromForm]GenerateDto dto)
        {
            try
            {
                //beállítja a semestert
                SemesterType sem = dto.SemesterType == "2" ? SemesterType.Spring : SemesterType.Autumn;
                //kitölti a kari hök nevét
                dto.facultyName = GetFacultyName(dto.faculty);
                //kitöltjük a hónap nevét
                dto.MonthName = GetMonthName(dto.Month);
                //kitölti az idei félévet
                dto.Semester = sem == SemesterType.Autumn ? $"{DateTime.Now.Year}/{DateTime.Now.AddYears(1).Year.ToString().Remove(0, 2)}/1" : $"{DateTime.Now.AddYears(-1).Year}/{DateTime.Now.Year.ToString().Remove(0, 2)}/2";

                dto.studentsInGroup = GetStudentsInGroup(sem, dto.faculty);

                //ebben tároljuk a letöltendő fájlok elérési útját
                List<string> resStrings = new List<string>();
                //kiszedjük egy listába azokat a hallgatókat akik kapnak ösztöndíjat
                dto.studentGetStipend = new List<StudentGenerateDto>();

                foreach (var item in dto.studentsInGroup)
                {
                    foreach (var stud in item.Value)
                    {
                        if (stud.StipendAmmount > 0)
                        { dto.studentGetStipend.Add(stud); }
                    }
                }

                //tanulmányi összegét kiszámolja
                dto.StipendSum = dto.studentGetStipend.Sum(q => q.StipendAmmount);

                if (dto.file != null)
                {
                    //elmenti a feltöltött közéletis táblát
                    string filename = dto.file.FileName;
                    var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Resources", dto.Uniid);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var ReadStream = new FileStream(Path.Combine(path, filename), FileMode.Create);
                    dto.file.CopyTo(ReadStream);
                    //beolvassuk a közéletis excel-t
                    List<StudentGenerateDto> StudentsWhoGetsKözéleti = ReadExcel.ReadKözéletiFile(Path.Combine(path, filename));
                    foreach (StudentGenerateDto student in StudentsWhoGetsKözéleti)
                    {
                        var stud = dto.studentGetStipend.Where(q => q.NeptunCode == student.NeptunCode).FirstOrDefault();
                        if (stud != null)
                        {
                            stud.PublicStipAmmount = student.PublicStipAmmount;
                            stud.SinglePublicStipAmmount = student.SinglePublicStipAmmount;
                            stud.PublicStipReason = student.PublicStipReason;
                        }
                        else
                        {
                            var studFromDb = _studRepo.GetStudentFromDB(student.NeptunCode);
                            var createdStud = _mapper.Map<StudentGenerateDto>(studFromDb.Result);
                            createdStud.PublicStipAmmount = student.PublicStipAmmount;
                            createdStud.SinglePublicStipAmmount = student.SinglePublicStipAmmount;
                            createdStud.PublicStipReason = student.PublicStipReason;
                            dto.studentGetStipend.Add(createdStud);
                        }
                    }
                    //közéleti összegét kiszámolja
                    dto.KözeletiSum = dto.studentGetStipend.Sum(q => q.PublicStipAmmount) + dto.studentGetStipend.Sum(y => y.SinglePublicStipAmmount);
                    resStrings.Add(FileGeneratorFactory.GenerateKözeletiBizonylat(dto));
                    resStrings.Add(FileGeneratorFactory.GenerateKözUtalas(dto));
                    resStrings.Add(FileGeneratorFactory.GenerateKözeletiÖsszesitö(dto));
                }

                resStrings.Add(FileGeneratorFactory.GenerateNeptunba_Excel(dto));
                resStrings.Add(FileGeneratorFactory.GenerateNyomtatniForm_Excel(dto));

                resStrings.Add(FileGeneratorFactory.GenerateTanulmányiBizonylat(dto));
                resStrings.Add(FileGeneratorFactory.GenerateTanUtalas(dto));
                resStrings.Add(FileGeneratorFactory.GenerateTanulmanyiÖsszesito_Word(dto));
                //itt még be kell majd zippelni az egészet
                //FileStream resFileStream = new FileStream(res, FileMode.Open, FileAccess.Read);
                ZipFile zip = new ZipFile();

                //foreach (var item in resStrings)
                //{
                //    zip.UseUnicodeAsNecessary = true;
                //    zip.AddFile(item, "./");
                //}
                zip.UseUnicodeAsNecessary = true;
                zip.AddFiles(resStrings, "./");
                string savedirectory = Path.Combine(
                    Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                    "Resources", dto.Uniid, dto.Roleid);
                string savepath = Path.Combine(savedirectory, $"{dto.facultyName}_{dto.Month}_tanulmanyi.zip");

                zip.Save(savepath);

                return DownloadFileAsync(savepath).Result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private Dictionary<string,List<StudentGenerateDto>> GetStudentsInGroup(SemesterType semester, string faculty)
        {
            Dictionary<string, List<StudentGenerateDto>> res = new Dictionary<string, List<StudentGenerateDto>>();
            var studsInGroup = _studRepo.GetGrouppedStudents(semester, faculty);
            foreach (var item in studsInGroup)
            {
                KeyValuePair<string, List<StudentGenerateDto>> group = new KeyValuePair<string, List<StudentGenerateDto>>(item.Key, new List<StudentGenerateDto>());
                foreach (var student in item.Value)
                {
                    group.Value.Add(_mapper.Map<StudentGenerateDto>(student));
                }
                res.Add(group.Key, group.Value);
            }
            return res;
        }

        private List<StudentGenerateDto> ConvertDictionaryToList(Dictionary<string,List<StudentGenerateDto>> input)
        {
            List<StudentGenerateDto> res = new List<StudentGenerateDto>();
            foreach (var item in input)
            {
                foreach (var stud in item.Value)
                {

                    res.Add(stud);
                }
            }
            return res;
        }

        private async Task<FileStreamResult> DownloadFileAsync(string path)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetMimeType(path), path);
        }

        private string GetMimeType(string file)
        {
            string extension = Path.GetExtension(file).ToLowerInvariant();
            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".docx": return "application/vnd.ms-word";
                case ".xlsx": return "application/vnd.ms-excel";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".csv": return "text/csv";
                case ".zip": return "application/zip";
                default: return "";
            }
        }

        private string GetMonthName(string month)
        {
            switch (month)
            {
                case "01": return "január";
                case "02": return "február";
                case "03": return "március";
                case "04": return "április";
                case "05": return "május";
                case "06": return "június";
                case "09": return "szeptember";
                case "10": return "október";
                case "11": return "november";
                case "12": return "december";
                case "09-10" :return "szeptember-október";
                case "02-03" : return "február-március";

                default: return "";
            }
        }

        private string GetFacultyName(string facultyCode)
        {
            switch (facultyCode)
            {
                case "A": return "OE AMK HÖK";
                case "B": return "OE BGK HÖK";
                case "G": return "OE KGK HÖK";
                case "K": return "OE KVK HÖK";
                case "N": return "OE NIK HÖK";
                case "R": return "OE RKK HÖK";

                default: return "";
            }
        }

    }
}
