// <copyright file="KgkLogic.cs" company="PlaceholderCompany">
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

    public class KgkLogic : ILogic
    {
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            KgkLogic kl = new KgkLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "GI"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "MM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "GM"))
            {
                res.Add(item.Key, item.Value);
            }

            var gazdmen_Third = res.Where((arg) => arg.Key.Contains("GMF 3")).ToDictionary(s => s.Key, w => w.Value);
            var gazdmen_fourth = res.Where((arg) => arg.Key.Contains("GMF 4")).ToDictionary(s => s.Key, w => w.Value);
            var mm_Third = res.Where((arg) => arg.Key.Contains("MM 3")).ToDictionary(s => s.Key, w => w.Value);
            var mm_fourth = res.Where((arg) => arg.Key.Contains("MM 4")).ToDictionary(s => s.Key, w => w.Value);

            if (gazdmen_Third.Count != 0)
            {
                var merged_third = mm_Third.ElementAt(0).Value.Concat(gazdmen_Third.ElementAt(0).Value);
                res.Remove(gazdmen_Third.ElementAt(0).Key);
                res.Remove(mm_Third.ElementAt(0).Key);
                res.Add(mm_Third.ElementAt(0).Key, merged_third.ToList());
            }

            if (gazdmen_fourth.Count != 0)
            {
                var merged_fourth = mm_fourth.ElementAt(0).Value.Concat(gazdmen_fourth.ElementAt(0).Value);
            res.Remove(gazdmen_fourth.ElementAt(0).Key);
            res.Remove(mm_fourth.ElementAt(0).Key);
            res.Add(mm_fourth.ElementAt(0).Key, merged_fourth.ToList());
            }

            foreach (var item in kl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "KM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in kl.GetMScStudentsOfDepartment(studentsOfFaculty))
            {
                res.Add(item.Key, item.Value);
            }

            // var fourth = res.Where(p => p.Key.Contains("4") && p.Key.Contains("KMF")).Select(s => s).ToDictionary(q => q.Key, w => w.Value);
            // var third = res.Where(p => p.Key.Contains("3") && p.Key.Contains("KMF")).Select(s => s).ToDictionary(q => q.Key, w => w.Value);
            // var merged = fourth.ElementAt(0).Value.Concat(third.ElementAt(0).Value).ToList();
            // res.Add("KMF 3+ aktív félév", merged);
            // res.Remove(fourth.ElementAt(0).Key);
            // res.Remove(third.ElementAt(0).Key);
            return res;
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart)
        {
            var result = (from stud in studentsOfFaculty
                         group stud by(stud.ModulCode, stud.Year) into studbymodCode
                         select studbymodCode).ToDictionary(p => p.Key, q => q.ToList());

            var gazdinfo = result.Where(p => p.Key.ModulCode.StartsWith("GB") && p.Key.ModulCode.EndsWith("GI"));
            var műmen = result.Where(p => (p.Key.ModulCode.EndsWith("MM") || p.Key.ModulCode.EndsWith("MA")) && p.Key.ModulCode.StartsWith("GB"));

            List<Student> bscPeople = new List<Student>();
            switch (depart)
            {
                case "GI":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("GB") && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "MM":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("GB") && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("MA"))
                                 select student).ToList();
                    break;
                case "GM":
                    bscPeople = (from student in studentsOfFaculty
                                 where (student.ModulCode.StartsWith("GB") || student.ModulCode.StartsWith("GF")) && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 student.ModulCode.EndsWith(depart)
                                 select student).ToList();
                    break;
                case "KM":
                    bscPeople = (from student in studentsOfFaculty
                                 where (student.ModulCode.StartsWith("GB") || student.ModulCode.StartsWith("GF")) && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("KA"))
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
            Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
            var mScPeople = (from student in studentsOfFaculty
                             where student.ModulCode.StartsWith("GM") && student.FinancialState.Contains("Állami ösztöndíjas")
                             orderby student.StipendIndex descending
                             select student).ToList();
            var list = mScPeople.Where(p => p.FinishedSemester == 1).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list.Count > 0)
            {
                grouppedStudents.Add(BusinessLogic.GetSemesters(list), list);
            }

            var list2 = mScPeople.Where(p => p.FinishedSemester == 2).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list2.Count > 0)
            {
                grouppedStudents.Add(BusinessLogic.GetSemesters(list2), list2);
            }

            var list3 = mScPeople.Where(p => p.FinishedSemester >= 3).OrderByDescending(q => q.StipendIndex).Select(s => s).ToList();
            if (list3.Count > 0)
            {
                grouppedStudents.Add(BusinessLogic.GetSemesters(list3), list3);
            }

            // grouppedStudents.Add("MSc", mScPeople);
            return grouppedStudents;
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }
    }
}
