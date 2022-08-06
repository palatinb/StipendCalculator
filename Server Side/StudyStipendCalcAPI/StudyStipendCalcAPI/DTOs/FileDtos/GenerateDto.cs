using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StudyStipendCalcAPI.DTOs.StudentDtos;

namespace StudyStipendCalcAPI.DTOs.FileDtos
{
    public class GenerateDto
    {
        public IFormFile file { get; set; }

        //bizonylat és utaláskísérőhöz
        public string Tan_Temanumber { get; set; }
        public string Koz_Temanumber { get; set; }
        public string Funkcioterulet { get; set; }
        public int StipendSum { get; set; }
        public int KözeletiSum { get; set; }
        public string ETPay { get; set; }
        public string Semester { get; set; }

        //elnöki határozathoz és alárísokhoz
        public string PresidentName { get; set; }
        public string PresidentNeptun { get; set; }

        public string VicePresidentName { get; set; }
        public string VicePresidentNeptun { get; set; }
        public string VicePresidentPercent { get; set; }

        public string EfName { get; set; }
        public string EfNeptun { get; set; }
        public string EfPercent { get; set; }

        public string Ef2Name { get; set; }
        public string Ef2Neptun { get; set; }
        public string Ef2Percent { get; set; }

        public string PrName { get; set; }
        public string PrNeptun { get; set; }
        public string PrPercent { get; set; }

        public string GazdName { get; set; }
        public string GazdNeptun { get; set; }
        public string GazdPercent { get; set; }


        //melyik hónap utalásai mennek ki kb mindnenhova kell
        public string Month { get; set; }
        public string MonthName { get; set; }

        //egyetem id
        public string Uniid { get; set; }
        //role id
        public string Roleid { get; set; }
        //melyik szemeszterre számolunk ösztöndíjat
        public string SemesterType { get; set; }
        //?? nem tudom mi akart lenni
        public string faculty { get; set; }
        public string facultyName { get; set; }
        //iktatószámok
        public string ElnokiIktato { get; set; }
        public string Tan_SummaryIktatoszam { get; set; }
        public string Tan_BizonylatIkatoszam { get; set; }
        public string Kozeleti_BizonylatIktatoszam { get; set; }
        public string Kozeleti_SummaryIktatoszam { get; set; }

        //hallgatók homogén csoportba rendezve mutatókhoz, ÖCSIhez
        public Dictionary<string, List<StudentGenerateDto>> studentsInGroup { get; set; }
        public List<StudentGenerateDto> studentsInGroupList { get; set; }
        //hallgatók akik kapnak ösztöndíjat
        public List<StudentGenerateDto> studentGetStipend { get; set; }
    }
}
