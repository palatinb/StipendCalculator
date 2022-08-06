using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudyStipendCalcAPI.Models;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace StudyStipendCalcAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CalculatorDBContext _context;

        public UserRepository(CalculatorDBContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password, DateTime last_login)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            //updates last_login property
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return user; // auth successful
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, salt;
            CreatePasswordHash(password, out passwordHash, out salt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<List<User>> GetAllUser()
        {

            return await _context.User.ToListAsync();
        }

        public async Task<bool> UserExists(int id)
        {
            if (await _context.User.AnyAsync(p => p.Id == id))
                return true;
            return false;
        }

        public async Task<bool> UsernameExist(string username)
        {
            if (await _context.User.AnyAsync(p => p.Username == username))
                return true;
            return false;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> EditUserByAdmin(User user, string password)
        {
            User origiUser = await _context.User.FirstAsync(p => p.Id == user.Id);
            if (password != null)
            {
                byte[] passwordHash, salt;
                CreatePasswordHash(password, out passwordHash, out salt);
                
                if (!(passwordHash == origiUser.PasswordHash))
                {
                    origiUser.PasswordHash = passwordHash; origiUser.PasswordSalt = salt;
                    origiUser.Name = user.Name;
                    origiUser.Username = user.Username;
                    origiUser.UiD = user.UiD;
                    origiUser.RoleId = user.RoleId;
                    _context.Entry(origiUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!(user.PasswordHash == origiUser.PasswordHash))
                {
                    origiUser.Name = user.Name;
                    origiUser.Username = user.Username;
                    origiUser.UiD = user.UiD;
                    origiUser.RoleId = user.RoleId;

                    _context.Entry(origiUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                User userToDelete = await _context.User.FirstAsync(z => z.Id == id);
                _context.User.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task<bool> CheckOldPassw(int id,string passw)
        {
            var user = await  _context.User.FirstAsync(p => p.Id == id);
            if (VerifyPasswordHash(passw, user.PasswordHash, user.PasswordSalt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EditUser(User user, string password)
        {
            byte[] passwordHash, salt;
            CreatePasswordHash(password, out passwordHash, out salt);
            User origiUser = await _context.User.FirstAsync(p => p.Id == user.Id);
            if (!(passwordHash == origiUser.PasswordHash))
            {
                origiUser.PasswordHash = passwordHash;
                origiUser.PasswordSalt = salt;
                _context.Entry(origiUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}