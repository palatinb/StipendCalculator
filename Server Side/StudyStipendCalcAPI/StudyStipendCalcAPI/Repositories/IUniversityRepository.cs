using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public interface IUniversityRepository
    {
        Task<bool> AddToDB(Universities stud);
        Task<Universities> GetUniFromDB(int id);
        Task<List<Universities>> GetAllUniFromDB();
        Task<bool> ModifyStudent(Universities uni);
        Task<bool> UniExist(int id);
        Task<bool> DeleteUni(int id);
    }
}
