using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.Old;
using Models.RequestResponses;

namespace Foosball.Logic
{
    public interface IAccountLogic
    {
        Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName);
        Task<LoginResult> ValidateLogin(string email, string token, string deviceName, string? role = null);
        Task<LoginResult> ValidateLogin(LoginSession loginSession, string? role = null);
        Task<GetUserMappingsResponse> GetUserMappings();
        Task<bool> ChangeUserPassword(string email, string newPassword);
        Task<bool> ChangeUserRoles(string userEmail, List<string> updatedRoles);
        Task<bool> Logout(LoginSession session);
        Task<bool> CreateUser(string email, string displayName, string password);
        Task<bool> RequestPassword(string email);
        Task<bool> ChangeEmail(string existingEmail, string newEmail);
        Task<bool> UserHasRole(string email, ClaimRoles role);
    }
}