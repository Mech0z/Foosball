using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.Middleware;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestResponses;

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
            if (loginRequest == null) throw new ArgumentNullException(nameof(loginRequest));
            if (loginRequest.Email == null) throw new ArgumentNullException(nameof(loginRequest.Email));
            if (loginRequest.DeviceName == null) throw new ArgumentNullException(nameof(loginRequest.DeviceName));
            if (loginRequest.Password == null) throw new ArgumentNullException(nameof(loginRequest.Password));

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
                Roles = result.Roles,
                LoginFailed = result.LoginFailed
            };
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Player)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            var loginSession = HttpContext.GetLoginSession();

            if (request.NewEmail == null || !IsValidEmail(request.NewEmail))
            {
                return BadRequest();
            }

            var existingUserOnNewEmail = await _accountLogic.GetUser(request.NewEmail);

            if (existingUserOnNewEmail != null)
            {
                return BadRequest();
            }

            var result = await _accountLogic.ChangeEmail(loginSession.Email, request.NewEmail);
            
            if (result)
                return Ok();

            return BadRequest();
        }

        private bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Unauth)]
        public async Task<bool> Logout()
        {
            return await _accountLogic.Logout(HttpContext.GetLoginSession());
        }
    }
}