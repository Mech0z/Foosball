using System.Collections.Generic;
using System.Threading.Tasks;
using Foosball.RequestResponse;
using Models.Old;

namespace Foosball.Logic
{
    public interface IAccountLogic
    {
        Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName);
        Task<LoginResult> ValidateLogin(BaseRequest request);
        Task<GetUserMappingsResponse> GetUserMappings(GetUserMappingsRequest request);
        Task<bool> ChangeUserPassword(string email, string newPassword);
        Task<bool> ChangeUserRoles(string userEmail, List<string> updatedRoles);
        Task<bool> Logout(BaseRequest request);
    }
}