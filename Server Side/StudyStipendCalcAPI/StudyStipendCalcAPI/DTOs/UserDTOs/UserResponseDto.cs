using System;
namespace StudyStipendCalcAPI.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}


