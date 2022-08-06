using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using StudyStipendCalcAPI.DTOs.StudentDtos;
namespace StudyStipendCalcAPI.Services.StudentServices
{
    public static class ReadExcel
    {
        public static List<StudentDto> ReadTanulmanyiExcelFile(int uid, string path)
        {
            //DataTable dt = new DataTable();
            List<StudentDto> students = new List<StudentDto>();
            var package = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                StudentDto student = new StudentDto()
                {
                    NeptunCode = workSheet.Cells[rowNumber, 1].Value.ToString(),
                    ModulCode = workSheet.Cells[rowNumber, 2].Value.ToString(),
                    ModulName = workSheet.Cells[rowNumber, 3].Value.ToString(),
                    TelephelyName = workSheet.Cells[rowNumber, 4].Value == null ? String.Empty : workSheet.Cells[rowNumber, 4].Value.ToString(),
                    Year = workSheet.Cells[rowNumber, 5].Value == DBNull.Value ? -1 : Convert.ToInt32(workSheet.Cells[rowNumber, 5].Value),
                    StudentGrop = (workSheet.Cells[rowNumber, 6].Value == null) ? String.Empty : workSheet.Cells[rowNumber, 6].Value.ToString(),
                    ActiveSemester = Convert.ToInt32(workSheet.Cells[rowNumber, 8].Value),
                    PassiveSemester = Convert.ToInt32(workSheet.Cells[rowNumber, 9].Value),
                    CreditIndex = Convert.ToDouble(workSheet.Cells[rowNumber, 10].Value),
                    EarnedCredit = Convert.ToInt32(workSheet.Cells[rowNumber, 12].Value),
                    AccceptedCredit = workSheet.Cells[rowNumber, 13].Value == DBNull.Value ? 0 : Convert.ToInt32(workSheet.Cells[rowNumber, 13].Value),
                    FinancialState = workSheet.Cells[rowNumber, 15].Value.ToString(),
                    Uid = uid
                };
                CalculateStudentFields(student);
                students.Add(student);
            }
            return students;
        }

        public static List<StudentGenerateDto> ReadKözéletiFile(string path)
        {
            List<StudentGenerateDto> students = new List<StudentGenerateDto>();
            var package = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                StudentGenerateDto student = new StudentGenerateDto()
                {
                    NeptunCode = workSheet.Cells[rowNumber, 1].Value.ToString(),
                    PublicStipAmmount = workSheet.Cells[rowNumber, 2].Value != null ? Int32.Parse(workSheet.Cells[rowNumber, 2].Value.ToString()) : 0,
                    SinglePublicStipAmmount = workSheet.Cells[rowNumber, 3].Value == null ? 0 : Int32.Parse(workSheet.Cells[rowNumber, 3].Value.ToString()),
                    PublicStipReason = workSheet.Cells[rowNumber, 4].Value == null ? String.Empty : workSheet.Cells[rowNumber, 4].Value.ToString()
                };
                students.Add(student);
            }
            return students;
        }

        public static void CalculateStudentFields(StudentDto stud)
        {
            stud.FinishedSemester = stud.ActiveSemester - 1;
            if (stud.FinishedSemester >= 7)
            {
                stud.ExceptedCredit = 210;
            }
            else
            {
                stud.ExceptedCredit = stud.FinishedSemester * 30;
            }
            stud.AllSemesters = stud.PassiveSemester + stud.ActiveSemester;
            //teljesített /elvárt szorzó
            double m = (stud.EarnedCredit / stud.ExceptedCredit);
            stud.StipendIndex = Math.Round(stud.CreditIndex * m,4);
            stud.YearOfEnrollment = YearOfEnr(stud);
            if (stud.Year == -1)
            {
                stud.Year = stud.FinishedSemester / 2;
            }
        }
        public static void CalculateFieldsOnModify(StudentDto student)
        {
            double m = (student.EarnedCredit / student.ExceptedCredit);
            student.StipendIndex = Math.Round(student.CreditIndex * m, 4);
        }
        private static string YearOfEnr(StudentDto stud)
        {
            string year = string.Empty;
            if (!(stud.AllSemesters % 2 == 0))
            {
                // ÖSSZEFŰZ(2019-KEREK.LE($T3/2;0);"/";19-KEREK.LE($T3/2;0)+1;"/";1)
                year += DateTime.Now.Year - (stud.AllSemesters / 2) + "/" +
                    (int.Parse(DateTime.Now.Year.ToString().Substring(2)) - (stud.AllSemesters / 2) + 1) + "/1";
            }
            else
            {
                year += DateTime.Now.Year - (stud.AllSemesters / 2) + "/" +
                    (int.Parse(DateTime.Now.Year.ToString().Substring(2)) - (stud.AllSemesters / 2) + 1) + "/2";
            }

            return year;
        }
    }
}