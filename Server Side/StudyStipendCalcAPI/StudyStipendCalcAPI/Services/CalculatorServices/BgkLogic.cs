// <copyright file="BgkLogic.cs" company="PlaceholderCompany">
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

    public class BgkLogic : ILogic
    {
        public static Dictionary<string, List<Student>> GetAllStudentByGropped(List<Student> studentsOfFaculty, SemesterType semester)
        {
            BgkLogic bl = new BgkLogic();
            Dictionary<string, List<Student>> res = new Dictionary<string, List<Student>>();
            foreach (var item in bl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "BT"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in bl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "GM"))
            {
                res.Add(item.Key, item.Value);
            }

            foreach (var item in bl.GetBScStudentsOfDepartment(studentsOfFaculty, semester, "ME"))
            {
                res.Add(item.Key, item.Value);
            }

            // stipisek az MSc-sek
            // foreach (var item in bl.GetMScStudentsOfDepartment(studentsOfFaculty))
            // {
            //    res.Add(item.Key, item.Value);
            // }
            return res;
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart)
        {
            List<Student> bscPeople = new List<Student>();

            if (depart == "BT")
            {
                bscPeople = (from student in studentsOfFaculty
                             where student.ModulCode.StartsWith("BB") && student.FinancialState.Contains("Állami ösztöndíjas") && student.ModulCode.EndsWith(depart)
                             select student).ToList();
            }
            else if (depart == "GM")
            {
                bscPeople = (from student in studentsOfFaculty
                             where student.ModulCode.StartsWith("BB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("GA"))
                             select student).ToList();
            }
            else if (depart == "ME")
            {
                bscPeople = (from student in studentsOfFaculty
                             where student.ModulCode.StartsWith("BB") && student.FinancialState.Contains("Állami ösztöndíjas") && (student.ModulCode.EndsWith(depart) || student.ModulCode.EndsWith("MA"))
                             select student).ToList();
            }

            return BusinessLogic.GroupStud(bscPeople, semester);
        }

        public Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart, IList telephely)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> allstudents)
        {
            throw new NotImplementedException();

        // Dictionary<string, List<Student>> grouppedStudents = new Dictionary<string, List<Student>>();
        // var mScPeople = (from student in allstudents
        //                     where student.ModulCode.StartsWith("BM") && student.FinancialState.Contains("Állami ösztöndíjas")
        //                     orderby student.ÖsztöndíjMutato descending
        //                     select student).ToList();
        //    grouppedStudents.Add(Student.GetSemesters(mScPeople), mScPeople);
        //    return grouppedStudents;
        }

        public Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> allstudents, SemesterType semester, IList depart)
        {
            throw new NotImplementedException();
        }
    }
}
