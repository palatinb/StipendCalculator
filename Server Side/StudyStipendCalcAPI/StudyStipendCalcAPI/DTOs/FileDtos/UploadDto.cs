using System;
using Microsoft.AspNetCore.Http;

namespace StudyStipendCalcAPI.DTOs.FileDtos
{
    public class UploadDto
    {
        public IFormFile file { get; set; }
        public string uni { get; set; }
    }
}
