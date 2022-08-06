// <copyright file="RkkLogic.cs" company="PlaceholderCompany">
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

    public class RkkLogic : ILogic
    {
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            RkkLogic rl = new RkkLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in rl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "forma"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in rl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "környezet"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in rl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "könnyű"))
            {
                res.Add(item.Key, item.Value);
            }

            if (semester == SemesterType.Spring)
            {
                var fourth = res.Where(p => p.Key.Contains("4")).Select(s => s).ToDictionary(q => q.Key, w => w.Value);
                var merged = fourth.ElementAt(0).Value.Concat(fourth.ElementAt(1).Value).Concat(fourth.ElementAt(2).Value).ToList();
                res.Add("ITF-KIP-KÖM 4. évesek", merged);

                foreach (var item in fourth)
                {
                    res.Remove(item.Key);
                }
            }
            
            foreach (var item in rl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "FOSZK"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in rl.GetMScStudentsOfDepartment(studentsOfFaculty))
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
                case "forma":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("RB") && student.FinancialState.Contains("Állami ösztöndíjas") &&
                                 student.ModulCode.EndsWith("TF")
                                 select student).ToList();
                    break;
                case "környezet":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("RB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith("KV")
                                 select student).ToList();
                    break;
                case "könnyű":
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("RB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith("KM")
                                 select student).ToList();
                    break;
                default:
                    bscPeople = (from student in studentsOfFaculty
                                 where student.ModulCode.StartsWith("RF") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith("AB") || student.ModulCode.EndsWith("AS"))
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
                             where student.ModulCode.StartsWith("RM") && student.FinancialState.Contains("Állami ösztöndíjas")
                             orderby student.StipendIndex descending
                             select student).ToList();
            grouppedStudents.Add(BusinessLogic.GetSemesters(mScPeople), mScPeople);
            return grouppedStudents;
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }
    }
}
