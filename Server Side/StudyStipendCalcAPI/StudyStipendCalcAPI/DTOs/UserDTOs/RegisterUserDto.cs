using System;
using System.ComponentModel.DataAnnotations;

namespace StudyStipendCalcAPI.DTOs.UserDTOs
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be at least 3 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "You must provide password between 8 and 20 characters")]
        public string PasswordHash { get; set; }
        public int UiD { get; set; }
        public int RoleId { get; set; }
        public DateTime Last_login { get; set; }

    }
}
