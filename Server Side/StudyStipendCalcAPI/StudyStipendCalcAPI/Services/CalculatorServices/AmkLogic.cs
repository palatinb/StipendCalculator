// <copyright file="AmkLogic.cs" company="PlaceholderCompany">
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

    public class AmkLogic : ILogic
    {
        // public static Dictionary<int, int[]> Spring_YearsToSemesters = new Dictionary<int, int[]>()
        // {
        //    { 1, new int[] { 1 } },
        //    { 2, new int[] { 2, 3 } },
        //    { 3, new int[] { 4, 5 } },
        //    { 4, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 } }
        // };

        // public static Dictionary<string, string[]> DepartmentPairWithModCode = new Dictionary<string, string[]>()
        // {
        //    { "Földmérő", new string[] {"ABNDFF", "ABNEFF" } },
        //    { "Gépész", new string[] {"ABNEGM" } },
        //    { "Mérnökinfo", new string[] {"ABNDMI", "ABNEMI" } },
        //    { "Műmen", new string[] {"ABNDMM", "ABNEMM" } },
        //    { "Villamos", new string[] {"ABNDVM", "ABNEVM" } },
        //    { "Foszk", new string[] { "AFNDMI", "AFNEMI" } },
        // };
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            AmkLogic amkLogic = new AmkLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "FF"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "MM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "GM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "VM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "MI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in amkLogic.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "FOSZk"))
            {
                res.Add(item.Key, item.Value);
            }

            return res;
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart)
        {
            List<Student> bscPeople = new List<Student>();
            switch (depart)
            {
                case "FF":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "MM":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "GM":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "MI":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "VM":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                default:
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("AF") && student.FinancialState.Contains("Állami ösztöndíjas")
                                 select student).ToList();
                    break;
            }

            return BusinessLogic.GroupStud(bscPeople, semester);
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart, IList telephely)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }
    }
}
