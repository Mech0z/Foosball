using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
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
                    Token = result.LoginToken.Token
                };
            }

            return new LoginResponse {LoginFailed = true};
        }
    }
}