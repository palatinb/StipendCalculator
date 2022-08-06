using System;
namespace StudyStipendCalcAPI.DTOs.StudentDtos
{
    public class StudentGenerateDto
    {
        public int Id { get; set; }
        public string NeptunCode { get; set; }
        public string ModulCode { get; set; }
        public string ModulName { get; set; }
        public string FinancialState { get; set; }
        public double StipendIndex { get; set; }
        public double GroupIndex { get; set; }
        public string YearOfEnrollment { get; set; }
        public int StipendAmmount { get; set; }
        public int PublicStipAmmount { get; set; }
        public int SinglePublicStipAmmount { get; set; }
        public string PublicStipReason { get; set; }
    }
}
