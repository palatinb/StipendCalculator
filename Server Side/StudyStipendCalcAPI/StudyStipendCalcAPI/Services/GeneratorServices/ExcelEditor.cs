using System.IO;
using System.Linq;
using OfficeOpenXml;
using StudyStipendCalcAPI.DTOs.FileDtos;

namespace StudyStipendCalcAPI.Services.FileServices
{
    public static class ExcelEditor
    {
        public static ExcelPackage OpenFile(string pathFolder, string filename)
        {
            string openpathFolder = Path.Combine(pathFolder, "Templates");
            if (!Directory.Exists(openpathFolder))
            {
                Directory.CreateDirectory(openpathFolder);
            }
            ExcelPackage ep = new ExcelPackage(new FileInfo(Path.Combine(openpathFolder, filename)));
            return ep;

        }
        public static string SaveFile(string pathfolder, string roleid, string uniid, string filename)
        {
            string savedirectory = Path.Combine(pathfolder, roleid);
            string savepath = Path.Combine(savedirectory, filename);
            if (!Directory.Exists(savedirectory))
            {
                Directory.CreateDirectory(savedirectory);
            }
            return savepath;
        }

        public static ExcelWorksheet FillNeptunbaExcelWithData(ExcelWorksheet worksheet, GenerateDto generateDto, string faculty)
        {
            string facultyHÖK = generateDto.facultyName.Substring(3, generateDto.facultyName.Length-3);
            var studentWithStudyStipend = generateDto.studentGetStipend.Where(q => q.StipendAmmount != 0).ToList();
            var studentsWithközeleti = generateDto.studentGetStipend.Where(q => q.PublicStipAmmount != 0 || q.SinglePublicStipAmmount != 0).ToList();
            for (int i = 0; i < studentWithStudyStipend.Count; i++)
            {
                var student = studentWithStudyStipend.ElementAt(i);
                worksheet.Cells[i + 2, 1].Value = student.NeptunCode;
                worksheet.Cells[i + 2, 2].Value = student.ModulCode;
                worksheet.Cells[i + 2, 3].Value = "tanulm_rendsz_" + generateDto.MonthName;
                worksheet.Cells[i + 2, 4].Value = student.YearOfEnrollment;
                worksheet.Cells[i + 2, 5].Value = generateDto.Semester;
                if (generateDto.Month.Split("-").Count() > 1)
                {
                    worksheet.Cells[i + 2, 6].Value = student.StipendAmmount * 2;
                }
                else
                {
                    worksheet.Cells[i + 2, 6].Value = student.StipendAmmount;
                }
                worksheet.Cells[i + 2, 9].Value = $"{faculty} Tanulmányi.ö. {generateDto.Semester} {generateDto.MonthName}";
                worksheet.Cells[i + 2, 10].Value = student.ModulName;
                worksheet.Cells[i + 2, 11].Value = student.FinancialState;
            }
            if (studentsWithközeleti.Count != 0)
            {
                for (int i = studentWithStudyStipend.Count; i < studentsWithközeleti.Count + studentWithStudyStipend.Count; i++)
                {
                    var student = studentsWithközeleti.ElementAt(i - studentWithStudyStipend.Count);
                    if (student.PublicStipAmmount != 0)
                    {
                        worksheet.Cells[i + 2, 1].Value = student.NeptunCode;
                        worksheet.Cells[i + 2, 2].Value = student.ModulCode;
                        worksheet.Cells[i + 2, 3].Value = "kozeleti_rendsz" + generateDto.Month;
                        worksheet.Cells[i + 2, 4].Value = student.YearOfEnrollment;
                        worksheet.Cells[i + 2, 5].Value = generateDto.Semester;
                        worksheet.Cells[i + 2, 6].Value = student.PublicStipAmmount;
                        worksheet.Cells[i + 2, 9].Value = $"{facultyHÖK} Közéleti.ö. Rendszeres {generateDto.Semester} {generateDto.MonthName}";
                        worksheet.Cells[i + 2, 10].Value = student.ModulName;
                        worksheet.Cells[i + 2, 11].Value = student.FinancialState;

                    }
                    if (student.SinglePublicStipAmmount != 0 && student.PublicStipAmmount == 0)
                    {
                        worksheet.Cells[i + 2, 1].Value = student.NeptunCode;
                        worksheet.Cells[i + 2, 2].Value = student.ModulCode;
                        worksheet.Cells[i + 2, 3].Value = "kozeleti_egyszeri" + generateDto.Month;
                        worksheet.Cells[i + 2, 4].Value = student.YearOfEnrollment;
                        worksheet.Cells[i + 2, 5].Value = generateDto.Semester;
                        worksheet.Cells[i + 2, 6].Value = student.SinglePublicStipAmmount;
                        worksheet.Cells[i + 2, 9].Value = $"{facultyHÖK} Közéleti.ö. egyszeri {generateDto.Semester} {generateDto.MonthName}";
                        worksheet.Cells[i + 2, 10].Value = student.ModulName;
                        worksheet.Cells[i + 2, 11].Value = student.FinancialState;
                    }
                    else if (student.SinglePublicStipAmmount != 0)
                    {
                        worksheet.Cells[i + 3, 1].Value = student.NeptunCode;
                        worksheet.Cells[i + 3, 2].Value = student.ModulCode;
                        worksheet.Cells[i + 3, 3].Value = "kozeleti_egyszeri" + generateDto.Month;
                        worksheet.Cells[i + 3, 4].Value = student.YearOfEnrollment;
                        worksheet.Cells[i + 3, 5].Value = generateDto.Semester;
                        worksheet.Cells[i + 3, 6].Value = student.SinglePublicStipAmmount;
                        worksheet.Cells[i + 3, 9].Value = $"{facultyHÖK} Közéleti.ö. egyszeri {generateDto.Semester} {generateDto.MonthName}";
                        worksheet.Cells[i + 3, 10].Value = student.ModulName;
                        worksheet.Cells[i + 3, 11].Value = student.FinancialState;
                    }
                }
            }
            return worksheet;
        }
    }
}
