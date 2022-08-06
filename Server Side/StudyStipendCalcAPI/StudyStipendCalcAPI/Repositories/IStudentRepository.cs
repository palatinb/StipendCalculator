using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyStipendCalcAPI.DTOs;
using StudyStipendCalcAPI.DTOs.StudentDtos;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<bool> AddToDB(Student stud);
        Task<Student> GetStudentFromDB(string neptun);
        Task<bool> ModifyStudent(Student student);
        Task<List<Student>> GetAllStud();
        Task<List<Student>> GetFacultyDtudents(string faculty);
        Task<List<Student>> FilterStudent(int roleid, int unid, Dictionary<int, Dictionary<int, string[]>> UniRoleToFaculty);
        Dictionary<string, List<Student>> GetGrouppedStudents(SemesterType semesterType, string faculty);
    }
}
