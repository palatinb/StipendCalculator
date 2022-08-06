using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly CalculatorDBContext _context;
        public UniversityRepository(CalculatorDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToDB(Universities uni)
        {
            try
            {
                await _context.Universities.AddAsync(uni);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<Universities>>GetAllUniFromDB()
        {
            return await _context.Universities.ToListAsync();
        }

        public async Task<Universities> GetUniFromDB(int id)
        {
            return await _context.Universities.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UniExist(int id)
        {
            if (await _context.Universities.AnyAsync(p => p.Id == id))
                return true;
            return false;
        }

        public async Task<bool> ModifyStudent(Universities uni)
        {
            if (await GetUniFromDB(uni.Id) == null)
            {
                return false;
            }
            else
            {
                 _context.Universities.Update(uni);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteUni(int id)
        {
            try
            {
                Universities uniToDelete = await _context.Universities.FirstAsync(z => z.Id == id);
                _context.Universities.Remove(uniToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
