// <copyright file="KvkLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace StudyStipendCalcAPI.Services.StudentServices.CalculatorServices

{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using StudyStipendCalcAPI.DTOs.StudentDtos;
    using StudyStipendCalcAPI.Models;

    public class KvkLogic : ILogic
    {
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            KvkLogic kl = new KvkLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            if (semester == SemesterType.Spring)
            {
                res = SpringType(studentsOfFaculty, semester, kl);
            }
            else
            {
                res = AutumnType(studentsOfFaculty, semester, kl);
            }

            return res;
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            List<Student> bscPeople = new List<Student>();
            if (semester == SemesterType.Spring)
            {
                switch (depart)
                {
                    case "AUT":
                        bscPeople = (from student in studentsOfFaculty
                                     where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                     select student).ToList();
                        break;
                    case "HTI":
                        bscPeople = (from student in studentsOfFaculty
                                     where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("GA"))
                                     select student).ToList();
                        break;
                    case "MAI":
                        bscPeople = (from student in studentsOfFaculty
                                     where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("MA"))
                                     select student).ToList();
                        break;
                    case "MTI":
                        bscPeople = (from student in studentsOfFaculty
                                     where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("MA"))
                                     select student).ToList();
                        break;
                    case "VEI":
                        bscPeople = (from student in studentsOfFaculty
                                     where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("MA"))
                                     select student).ToList();
                        break;
                }
            }
            else
            {
                // őszi félév modul szerint
            }

            grouppedStudents.Add(BusinessLogic.GetSemesters(bscPeople), bscPeople);
            return grouppedStudents;
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart, string telephely)
        {
            List<Student> bscPeople = new List<Student>();
            bscPeople = (from student in studentsOfFaculty
                         where student.ModulCode.StartsWith("KB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.TelephelyName.Contains(telephely) && student.StudentGrop.Contains(depart)
                         select student).ToList();

            return BusinessLogic.GroupStud(bscPeople, semester);
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var mScPeople = (from student in studentsOfFaculty
                             where (student.ModulCode.StartsWith("KB") || student.ModulCode.StartsWith("KM")) && student.FinancialState.Contains("Állami ösztöndíjas")
                             orderby student.StipendIndex descending
                             select student).ToList();
            grouppedStudents.Add("MSc", mScPeople);
            return BusinessLogic.GroupStud(mScPeople, SemesterType.Autumn);
        }

        public Dictionary<string, List<Student>> GetEnglishStudentsOfDepartment(List<Student> studentsOfFaculty)
        {
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var engPeople = (from student in studentsOfFaculty
                             where student.ModulCode.EndsWith("A") && student.FinancialState.Contains("Állami ösztöndíjas")
                             orderby student.StipendIndex descending
                             select student).ToList();
            grouppedStudents.Add("English", engPeople);
            return BusinessLogic.GroupStud(engPeople, SemesterType.Autumn);
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTelephely(List<Student> students)
        {
             return (from ths in students
                    group ths by ths.TelephelyName into x
                    select x.Key).ToList();
        }

        private static Dictionary<string, List<Student>> AutumnType(List<Student> studentsOfFaculty, SemesterType semester, KvkLogic kl)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<string, List<Student>> SpringType(List<Student> studentsOfFaculty, SemesterType semester, KvkLogic kl)
        {
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "VI", "Tavasz"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "VI", "Bécs"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "AUT"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "HTI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "MAI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "MTI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "VEI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetEnglishStudentsOfDepartment(studentsOfFaculty))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetMScStudentsOfDepartment(studentsOfFaculty))
            {
                res.Add(item.Key, item.Value);
            }

            return res;
        }
    }
}
