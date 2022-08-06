using System;
using System.ComponentModel;

namespace StudyStipendCalcAPI.DTOs.StudentDtos
{
    public enum SemesterType
    { Spring, Autumn }
    public class StudentDto
    {
        public int Id { get; set; }
        public string NeptunCode { get; set; }
        public string ModulCode { get; set; }
        public string ModulName { get; set; }
        public string TelephelyName { get; set; }
        public int Year { get; set; }
        /// <summary>
        /// Tankör
        /// </summary>
        /// <value>The student grop.</value>
        public string StudentGrop { get; set; }
        public int ActiveSemester { get; set; }
        public int PassiveSemester { get; set; }
       
        /// <summary>
        /// Ösztöndíjindex
        /// </summary>
        /// <value>The index of the credit.</value>
        public double CreditIndex { get; set; }
        public double EarnedCredit { get; set; }
        public string FinancialState { get; set; }
        public int AccceptedCredit { get; set; }
        public int AllSemesters { get; set; }


        /// <summary>
        /// Ösztöndíjmutató
        /// </summary>
        /// <value>The index of the stipend.</value>
        public double StipendIndex { get; set; }
        /// <summary>
        /// ÖCSI
        /// </summary>
        /// <value>The index of the group.</value>
        public double GroupIndex { get; set; }
        public double ExceptedCredit { get; set; }
        public string YearOfEnrollment { get; set; }
        public int FinishedSemester { get; set; }
        /// <summary>
        /// Ösztöndíj összege
        /// </summary>
        /// <value>The stipend ammount.</value>
        public int StipendAmmount { get; set; }
        public int Uid { get; set; }
    }
}
