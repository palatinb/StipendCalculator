using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public interface IRoleRepository
    {

        Dictionary<int, Dictionary<int, string[]>> UniRoleToFaculty { get; }
        Task<bool> AddToDB(Roles role);
        Task<List<Roles>> GetAllRoles();
        Task<string> GetRoleName(int id);

    }
}
