using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.Middleware;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountLogic _accountLogic;

        public AccountController(IAccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        [HttpPost]
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var result = await _accountLogic.Login(loginRequest.Email,
                loginRequest.Password,
                loginRequest.RememberMe,
                loginRequest.DeviceName);

            if (result.Success)
            {
                return new LoginResponse
                {
                    LoginFailed = false,
                    ExpiryTime = result.LoginToken.Expirytime,
                    Token = result.LoginToken.Token,
                    Roles = result.Roles
                };
            }

            return new LoginResponse {LoginFailed = true};
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Player)]
        public async Task<LoginResponse> ValidateLogin()
        {
            var result = await _accountLogic.ValidateLogin(HttpContext.GetLoginSession());

            return new LoginResponse
            {
                Token = result.LoginToken.Token,
                ExpiryTime = result.LoginToken.Expirytime,
                LoginFailed = result.LoginFailed
            };
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Unauth)]
        public async Task<bool> Logout()
        {
            return await _accountLogic.Logout(HttpContext.GetLoginSession());
        }
    }
}