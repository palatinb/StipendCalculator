using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password, DateTime last_login);
        Task<bool> UserExists(int id);
        Task<bool> UsernameExist(string username);
        Task<bool> CheckOldPassw(int id, string passw);
        Task<List<User>> GetAllUser();
        Task<User> GetUserById(int id);
        Task<bool> EditUserByAdmin(User user,string password);
        Task<bool> EditUser(User user, string password);
        Task<bool> DeleteUser(int id);
    }
}