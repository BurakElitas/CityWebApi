using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiForAngular.Models;

namespace WebApiForAngular.Data
{
    public class AuthRepository : IAuthRepository
    {
        IRepository _repo;
        private DataContext _context;

        public AuthRepository(IRepository repo,DataContext context)
        {
            _repo = repo;
            _context = context;
            
        }
        public async Task<User> Login(string userName, string password)
        {
           var user = await _repo.FindAsync<User>(x => x.UserName == userName);
            if (user == null)
                return null;

            if(VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt)==false)
            {
                return null;
            }

            return user;
         }

   

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if(computeHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                 
                }
                return true;
            }

        }

     

        [HttpPost("Register")]
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await _repo.InsertAsync<User>(user);
            await _repo.SaveAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac=new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExists(string UserName)
        {
            if(await _repo.FindAsync<User>(x=>x.UserName==UserName)!=null)
            {
                return true;
            }
            return false;
        }
    }
}
