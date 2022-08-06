// <copyright file="ILogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace StudyStipendCalcAPI.Services.StudentServices.CalculatorServices

{
    using System.Collections;
    using System.Collections.Generic;
    using StudyStipendCalcAPI.DTOs.StudentDtos;
    using StudyStipendCalcAPI.Models;

    internal interface ILogic
    {
        Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, string depart);

        // Dictionary<string, List<Student>> GetBScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList telephely);
        Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty);

        Dictionary<string, List<Student>> GetMScStudentsOfDepartment(List<Student> studentsOfFaculty, SemesterType semester, IList depart);
    }
}
