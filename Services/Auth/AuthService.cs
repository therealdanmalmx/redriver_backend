using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.IdentityModel.Tokens;
using redriver_backend.Models;
using redriver_test.Data;
using redriver_test.Migrations;
using redriver_test.Models;

namespace redriver_test.Services.Auth
{
    public class AuthService : IAauthService
    {
        private readonly DataContext _db;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            _db = db;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var serviceResponse = new ServiceResponse<string>();

            var user = await _db.Users.FirstOrDefaultAsync(user => user.UserName.ToLower().Equals(username.ToLower()));

            if (user == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Username / password not found";
            }

            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Username / password not found";
            }
            else
            {
                serviceResponse.Data = CreateToken(user);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> Register(User user, string password)
        {
            var serviceResponse = new ServiceResponse<Guid>();

            if(await UserExists(user.UserName))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Username exists";
                return serviceResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user!.PasswordHash = passwordHash;
            user!.PasswordSalt = passwordSalt;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            serviceResponse.Data = user.Id;
            serviceResponse.Message = "User created successfully";
            return serviceResponse;

        }


        public async Task<bool> UserExists(string username)
        {
            if(await _db.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {

            var serviceResponse = new ServiceResponse<string>();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var appSettingToken = _configuration.GetSection("AppSettings:Token").Value;

            if (string.IsNullOrEmpty(appSettingToken)) {
                serviceResponse.Success = false;
                serviceResponse.Message = "Token is null";
                return serviceResponse.Message;
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingToken));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}