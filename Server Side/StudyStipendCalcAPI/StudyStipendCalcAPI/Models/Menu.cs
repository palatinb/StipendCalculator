using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Models
{
    public partial class Menu
    {
        public Menu()
        {
            LinkRolesMenus = new HashSet<LinkRolesMenus>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }

        public virtual ICollection<LinkRolesMenus> LinkRolesMenus { get; set; }
    }
}
