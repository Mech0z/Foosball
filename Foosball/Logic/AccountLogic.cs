using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Old;
using Models.RequestResponses;
using Repository;

namespace Foosball.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private readonly IUserLoginInfoRepository _userLoginInfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailLogic _emailLogic;

        public AccountLogic(IUserLoginInfoRepository userLoginInfoRepository, IUserRepository userRepository, IEmailLogic emailLogic)
        {
            _userLoginInfoRepository = userLoginInfoRepository;
            _userRepository = userRepository;
            _emailLogic = emailLogic;
        }

        public async Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName)
        {
            return await _userLoginInfoRepository.Login(email, password, rememberMe, deviceName);
        }

        public async Task<LoginResult> ValidateLogin(LoginSession loginSession, string? role = null)
        {
            var loginResult =
                await _userLoginInfoRepository.VerifyLogin(loginSession.Email, loginSession.Token, loginSession.DeviceName);

            if (role != null && !loginResult.Roles.Contains(role))
            {
                return new LoginResult{LoginFailed = true};
            }

            return loginResult;
        }

        public async Task<LoginResult> ValidateLogin(string email, string token, string deviceName, string? role = null)
        {
            LoginResult loginResult =
                await _userLoginInfoRepository.VerifyLogin(email, token, deviceName);

            if (role != null && !loginResult.Roles.Contains(role))
            {
                loginResult.LoginFailed = true;
                loginResult.Success = false;
                return loginResult;
            }

            return loginResult;
        }

        public async Task<GetUserMappingsResponse> GetUserMappings()
        {
            var users = await _userRepository.GetUsersAsync();
            var userRoles = await _userLoginInfoRepository.GetAllUserRoles();
            
            var response = new GetUserMappingsResponse();

            foreach (var user in users)
            {
                response.Users.Add(new UserMappingsResponseEntry(user.Email,
                    userRoles.SingleOrDefault(x => x.Email == user.Email)?.Roles));
            }

            return response;
        }

        public async Task<bool> ChangeUserPassword(string email, string newPassword)
        {
            var existingUsers = await _userRepository.GetUsersAsync();

            if (existingUsers.Any(x => x.Email == email))
            {
                return await _userLoginInfoRepository.AdminChangePassword(email, newPassword);
            }
            
            return false;
        }

        public async Task<User> GetUser(string email)
        {
            return await _userRepository.GetUser(email);
        }

        public async Task<bool> ChangeUserRoles(string userEmail, List<string> updatedRoles)
        {
            var existingUsers = await _userRepository.GetUsersAsync();

            if (existingUsers.Any(x => x.Email == userEmail))
            {
                return await _userLoginInfoRepository.UpdateUserRoles(userEmail, updatedRoles);
            }
            
            return false;
        }

        public async Task<bool> CreateUser(string email, string displayName, string password)
        {
            var createUserResult = await _userRepository.AddUser(email, displayName);

            if (!createUserResult)
                return false;

            var createUserLoginInfo = await _userLoginInfoRepository.CreateUser(email, password);

            return createUserLoginInfo;
        }

        public async Task<bool> RequestPassword(string email)
        {
            var newPassword = CreatePassword(6);

            var success = await _userLoginInfoRepository.AdminChangePassword(email, newPassword);
            var user = await _userRepository.GetUser(email);

            if (!success)
                return false;

            return await _emailLogic.SendEmailV2(email, user.Username, "Your new password", $"New password : {newPassword}");
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public async Task<bool> UserHasRole(string email, ClaimRoles role)
        {
            var roles = await _userLoginInfoRepository.GetUserRoles(email);
            return roles.Contains(role.ToString());
        }

        public async Task<bool> Logout(LoginSession session)
        {
            return await _userLoginInfoRepository.Logout(session.Email, session.Token, session.DeviceName);
        }

        public async Task<bool> ChangeEmail(string existingEmail, string newEmail)
        {
            return await _userRepository.ChangeEmail(existingEmail, newEmail);
        }
    }
}