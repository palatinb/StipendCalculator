using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Models
{
    public partial class Roles
    {
        public Roles()
        {
            LinkRolesMenus = new HashSet<LinkRolesMenus>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Uid { get; set; }

        public virtual Universities U { get; set; }
        public virtual ICollection<LinkRolesMenus> LinkRolesMenus { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
