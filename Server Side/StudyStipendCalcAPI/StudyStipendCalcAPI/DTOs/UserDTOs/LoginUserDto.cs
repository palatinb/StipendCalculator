using System;
using System.ComponentModel.DataAnnotations;

namespace StudyStipendCalcAPI.DTOs.UserDTOs
{
    public class LoginUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime Last_login { get; set; }
    }
}
