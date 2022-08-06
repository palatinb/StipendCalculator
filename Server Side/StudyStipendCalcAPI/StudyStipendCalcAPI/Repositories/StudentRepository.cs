using System;
using System.Threading.Tasks;
using System.Linq;
using StudyStipendCalcAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using StudyStipendCalcAPI.DTOs.StudentDtos;
using StudyStipendCalcAPI.Services.StudentServices.CalculatorServices;

namespace StudyStipendCalcAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CalculatorDBContext _context;
        

        public StudentRepository(CalculatorDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToDB(Student stud)
        {
            try
            {
                
                await _context.Student.AddAsync(stud);
                await _context.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<List<Student>> FilterStudent(int roleid, int uniID, Dictionary<int, Dictionary<int, string[]>> UniRoleToFaculty)
        {
            List<Student> res = new List<Student>();
            if (roleid == 1)
            {
                return await GetAllStud();
            }
            else
            {
                foreach (var item in UniRoleToFaculty.First(q=> q.Key== uniID).Value.First(p => p.Key == roleid).Value)
                {
                    res.AddRange(await (from stud in _context.Student
                                        where stud.ModulCode.StartsWith(item) && stud.Uid == uniID
                                        select stud).ToListAsync());
                }
            }
            return res;
        }

        public async Task<List<Student>> GetAllStud()
        {
            return await _context.Student.ToListAsync();
        }

        public async Task<List<Student>> GetFacultyDtudents(string faculty)
        {
            return await (from student in _context.Student
                    where student.ModulCode.StartsWith(faculty)
                    select student).ToListAsync();
        }

        public async Task<Student> GetStudentFromDB(string neptun)
        {
            Student stud;
            stud = await _context.Student.SingleOrDefaultAsync(p => p.NeptunCode == neptun);
            return stud;
        }

        public async Task<bool> ModifyStudent(Student student)
        {
            if (_context.Student.Where(p=> p.NeptunCode == student.NeptunCode).ToList().Count == 0)
            {
                return false;
            }
            else
            {
                try
                {
                    var origiStud = _context.Student.FirstOrDefault(p => p.NeptunCode == student.NeptunCode);
                    //keresésnél mósodulhat
                    origiStud.EarnedCredit = student.EarnedCredit;
                    origiStud.CreditIndex = student.CreditIndex;
                    origiStud.FinancialState = student.FinancialState;
                    origiStud.StipendIndex = student.StipendIndex;

                    //számolásnál módosulnak ezek
                    origiStud.GroupIndex = student.GroupIndex;
                    origiStud.StipendAmmount = student.StipendAmmount;

                    _context.Entry(origiStud).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public Dictionary<string, List<Student>> GetGrouppedStudents(SemesterType semesterType, string faculty)
        {
            Dictionary<string, List<Student>> grouppedStuds = new Dictionary<string, List<Student>>();
            // inputnál már parsolni kell hogy csak egy karakter jöjjön frontendtől
            List<Student> peopleOfFaculty = GetFacultyDtudents(faculty).Result;
            switch (faculty.ToUpper())
            {
                case "A": grouppedStuds = AmkLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                case "B": grouppedStuds = BgkLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                case "G": grouppedStuds = KgkLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                case "K": grouppedStuds = KvkLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                case "N": grouppedStuds = NikLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                case "R": grouppedStuds = RkkLogic.GetAllStudentByGropped(peopleOfFaculty, semesterType); break;
                default:
                    break;
            }
            return grouppedStuds;
        }

    }
}
