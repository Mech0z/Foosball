using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball.RequestResponse;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private readonly IUserLoginInfoRepository _userLoginInfoRepository;
        private readonly IUserRepository _userRepository;

        public AccountLogic(IUserLoginInfoRepository userLoginInfoRepository, IUserRepository userRepository)
        {
            _userLoginInfoRepository = userLoginInfoRepository;
            _userRepository = userRepository;
        }

        public async Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName)
        {
            return await _userLoginInfoRepository.Login(email, password, rememberMe, deviceName);
        }

        public async Task<LoginResult> ValidateLogin(BaseRequest request, string role = null)
        {
            var loginResult =
                await _userLoginInfoRepository.VerifyLogin(request.Email, request.Token, request.DeviceName);

            if (role != null && !loginResult.Roles.Contains(role))
            {
                return new LoginResult{LoginFailed = true};
            }

            return loginResult;
        }

        public async Task<LoginResult> ValidateLogin(string email, string token, string deviceName, string role = null)
        {
            var loginResult =
                await _userLoginInfoRepository.VerifyLogin(email, token, deviceName);

            if (role != null && !loginResult.Roles.Contains(role))
            {
                return new LoginResult{LoginFailed = true};
            }

            return loginResult;
        }

        public async Task<GetUserMappingsResponse> GetUserMappings(GetUserMappingsRequest request)
        {
            var users = await _userRepository.GetUsersAsync();
            var userRoles = await _userLoginInfoRepository.GetAllUserRoles();
            
            var response = new GetUserMappingsResponse();

            foreach (var user in users)
            {
                response.Users.Add(new UserMappingsResponseEntry
                {
                    Email = user.Email,
                    Roles = userRoles.SingleOrDefault(x => x.Email == user.Email)?.Roles
                });
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

        public async Task<bool> ChangeUserRoles(string userEmail, List<string> updatedRoles)
        {
            var existingUsers = await _userRepository.GetUsersAsync();

            if (existingUsers.Any(x => x.Email == userEmail))
            {
                return await _userLoginInfoRepository.UpdateUserRoles(userEmail, updatedRoles);
            }
            
            return false;
        }

        public async Task<bool> Logout(BaseRequest request)
        {
            return await _userLoginInfoRepository.Logout(request.Email, request.Token, request.DeviceName);
        }
    }
}