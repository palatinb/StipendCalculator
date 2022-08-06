// <copyright file="NikLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace StudyStipendCalcAPI.Services.StudentServices.CalculatorServices
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using StudyStipendCalcAPI.DTOs.StudentDtos;
    using StudyStipendCalcAPI.Models;

    public class NikLogic : ILogic
    {
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            NikLogic nl = new NikLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in nl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "üzem"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in nl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "mérnök"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in nl.GetMScStudentsOfDepartment(studentsOfFaculty))
            {
                res.Add(item.Key, item.Value);
            }

            return res;
        }

        /// <summary>
        /// Gets the bsc students of department.
        /// </summary>
        /// <param name="studentsOfFaculty">The students of faculty.</param>
        /// <param name="semester">The semester.</param>
        /// <param name="depart">The depart.</param>
        /// <returns>Returns a List.</returns>
        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
                List<Student> bscPeople = new List<Student>();

                if (depart != "üzem")
                {
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("NB") && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 (student.ModulCode.EndsWith("MI") || student.ModulCode.EndsWith("MA") || student.ModulCode.EndsWith("MS"))
                                 select student).ToList();
                }
                else
                {
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("NB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith("UM")
                                 select student).ToList();
                }

            return BusinessLogic.GroupStud(bscPeople, semester);
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart, IList telephely)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var mScPeople = (from student in studentsOfFaculty
                             where student.ModulCode.StartsWith("NM") && student.FinancialState.Contains("Állami ösztöndíjas")
                             orderby student.StipendIndex descending
                             select student).ToList();
            grouppedStudents.Add(BusinessLogic.GetSemesters(mScPeople), mScPeople);
            return grouppedStudents;
        }
    }
}
