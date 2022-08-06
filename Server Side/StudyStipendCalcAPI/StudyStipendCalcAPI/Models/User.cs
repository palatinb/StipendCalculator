using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int RoleId { get; set; }
        public int UiD { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Universities U { get; set; }
    }
}
