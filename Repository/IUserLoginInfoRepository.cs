﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IUserLoginInfoRepository
    {
        Task<LoginResult> VerifyLogin(string email, string token, string deviceName);
        Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName);
        Task Logout(string email, string token, string deviceName);
        Task<bool> ChangePassword(string email, string oldPassword, string newPassword);
        Task<bool> AdminChangePassword(string email, string newPassword);
        LoginToken CreateUser(string email, string token, string deviceName);
        Task<List<string>> GetUserRoles(string email);
        Task<List<UserRole>> GetAllUserRoles();
        Task<bool> UpdateUserRoles(string email, List<string> roles);
    }
}