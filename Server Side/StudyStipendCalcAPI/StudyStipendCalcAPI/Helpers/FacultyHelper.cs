using System;
using System.Collections.Generic;

namespace StudyStipendCalcAPI.Helpers
{
    public class FacultyHelper
    {
        public Dictionary<int, Dictionary<int, string[]>> UniRoleToFaculty = new Dictionary<int, Dictionary<int, string[]>>();
        public FacultyHelper()
        {
            PopulateDictrionary();
        }
        private void PopulateDictrionary()
        {
            Dictionary<int, string[]> RoleTOfaculty = new Dictionary<int, string[]>();
            //EHÖK OE
            RoleTOfaculty.Add(2, new string[] { "A", "B", "G", "K", "N", "R", "O" });
            //OE-NIK
            RoleTOfaculty.Add(3, new string[] { "N" });
            //OE-KGK
            RoleTOfaculty.Add(4, new string[] { "G" });
            //OE-KVK
            RoleTOfaculty.Add(5, new string[] { "K" });
            //OE-BGK
            RoleTOfaculty.Add(6, new string[] { "B" });
            //OE-AMK
            RoleTOfaculty.Add(7, new string[] { "A" });
            //OE-RKK
            RoleTOfaculty.Add(8, new string[] { "R" });
            foreach (var item in RoleTOfaculty)
            {
                UniRoleToFaculty.Add(1, RoleTOfaculty);
            }
        }
    }
}
