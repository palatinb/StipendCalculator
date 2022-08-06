using System;
using System.Collections.Generic;
using StudyStipendCalcAPI.DTOs.StudentDtos;

namespace StudyStipendCalcAPI.DTOs
{
    public class CalculateDto
    {
        public List<StudentDto> students;
        public int input;
        public double minStipendIndex;
        public int minPrice;
        public int maxPrice;
    }
}
