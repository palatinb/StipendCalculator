using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Models
{
    public partial class Universities
    {
        public Universities()
        {
            Roles = new HashSet<Roles>();
            Student = new HashSet<Student>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Dbname { get; set; }

        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
