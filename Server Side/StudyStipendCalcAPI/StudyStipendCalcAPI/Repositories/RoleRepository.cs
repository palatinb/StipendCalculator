using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CalculatorDBContext _context;
        private Dictionary<int, Dictionary<int, string[]>> uniRoleToFaculty = new Dictionary<int, Dictionary<int, string[]>>();
        public Dictionary<int, Dictionary<int, string[]>> UniRoleToFaculty { get { return uniRoleToFaculty; } }

        public RoleRepository(CalculatorDBContext context)
        {
            _context = context;
            PopulateDictrionary();
        }

        public async Task<bool> AddToDB(Roles role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public Task<string> GetRoleName(int id)
        {
            return _context.Roles.Where(p => p.Id == id).Select(q => q.Title).FirstOrDefaultAsync();
        }

        private void PopulateDictrionary()
        {
            Dictionary<int, string[]> RoleTOfaculty = new Dictionary<int, string[]>();
            RoleTOfaculty.Add(1, new string[] { "A", "B", "G", "K", "N", "R", "O" });
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
            uniRoleToFaculty.Add(1, RoleTOfaculty);
        }
    }
}
