using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string NeptunCode { get; set; }
        public string ModulCode { get; set; }
        public string ModulName { get; set; }
        public string TelephelyName { get; set; }
        public int Year { get; set; }
        public string StudentGrop { get; set; }
        public int ActiveSemester { get; set; }
        public int PassiveSemester { get; set; }
        public int FinishedSemester { get; set; }
        public int AllSemesters { get; set; }
        public double CreditIndex { get; set; }
        public double StipendIndex { get; set; }
        public double GroupIndex { get; set; }
        public int EarnedCredit { get; set; }
        public int ExceptedCredit { get; set; }
        public string FinancialState { get; set; }
        public string YearOfEnrollment { get; set; }
        public int AccceptedCredit { get; set; }
        public int StipendAmmount { get; set; }
        public int Uid { get; set; }

        public virtual Universities U { get; set; }
    }
}
