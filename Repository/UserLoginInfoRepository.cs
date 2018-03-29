﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class UserLoginInfoRepository : BaseRepository<UserLoginInfo>, IUserLoginInfoRepository
    {
        public UserLoginInfoRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "UserLoginInfos")
        {
        }

        public async Task<LoginResult> VerifyLogin(string email, string token, string deviceName)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

            var loginToken =
                existingUserLogin?.Tokens.SingleOrDefault(x => x.DeviceName == deviceName && x.Token == token);

            if (loginToken == null || loginToken.Expirytime < DateTime.Now) return new LoginResult{LoginFailed = true};

            if(loginToken.Expirytime > DateTime.Now.AddHours(1))
            {
                loginToken.Expirytime = DateTime.Now.AddDays(14);
            }

            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);
            
            return new LoginResult{Success = true, LoginToken = loginToken};
        }

        public async Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
            var correctPassword = BCrypt.Net.BCrypt.Verify(password, existingUserLogin.HashedPassword);

            if (!correctPassword) { return new LoginResult(); }
            
            var tokenGuid = Guid.NewGuid();
            var existingToken = existingUserLogin.Tokens.SingleOrDefault(x => x.DeviceName == deviceName);
            
            var expirytime = rememberMe ? DateTime.Now.AddDays(14) : DateTime.Now.AddMinutes(15);
            
            if (existingToken != null)
            {
                existingToken.Expirytime = expirytime;
                existingToken.Token = tokenGuid.ToString();
            }
            else
            {
                existingUserLogin.Tokens.Add(new LoginToken
                {
                    Token = tokenGuid.ToString(),
                    DeviceName = deviceName,
                    Expirytime = expirytime
                });
            }
            
            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);

            var newToken = existingUserLogin.Tokens.SingleOrDefault(x => x.Token == tokenGuid.ToString());
            
            return new LoginResult {Success = true, LoginToken = newToken};
        }

        public async Task Logout(string email, string token, string deviceName)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

            var existingToken =
                existingUserLogin?.Tokens.SingleOrDefault(x => x.DeviceName == deviceName && x.Token == token);

            if (existingToken != null)
            {
                existingUserLogin.Tokens.Remove(existingToken);
                await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);
            }
        }

        public async Task<bool> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
            var correctPassword = BCrypt.Net.BCrypt.Verify(oldPassword, existingUserLogin.HashedPassword);

            if (!correctPassword)
            {
                return false;
            }

            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            existingUserLogin.HashedPassword = newHash;
            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);
            return true;
        }

        public async Task<bool> AdminChangePassword(string email, string newPassword)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
            
            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            if (existingUserLogin == null)
            {
                var newUserLogin = new UserLoginInfo
                {
                    Email = email,
                    HashedPassword = newHash
                };
                await Collection.InsertOneAsync(newUserLogin);
            }
            else
            {
                existingUserLogin.HashedPassword = newHash;
                await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);
            }
            
            return true;
        }

        public LoginToken CreateUser(string email, string token, string deviceName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetUserRoles(string email)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

            return existingUserLogin?.Roles;
        }

        public async Task<List<UserRole>> GetAllUserRoles()
        {
            var userLoginInfos = await Collection.AsQueryable().ToListAsync();

            return userLoginInfos.Select(x => new UserRole(x)).ToList();
        }

        public async Task<bool> UpdateUserRoles(string email, List<string> roles)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

            if (existingUserLogin == null) {return false;}

            existingUserLogin.Roles = roles;
            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);

            return true;
        }
    }
}