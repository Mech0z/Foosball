using System;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[Authorize(Roles= "Admin")]
    [ApiController]
    public class AdministrationController : Controller
    {
        private readonly IAccountLogic _accountLogic;

        public AdministrationController(IAccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        [HttpPost]
        public async Task<GetUserMappingsResponse> GetUserMappings(GetUserMappingsRequest request)
        {
            var loginResult = await _accountLogic.ValidateLogin(request, "Admin");
            if (loginResult.Success)
            {
                return await _accountLogic.GetUserMappings(request);
            }

            throw new AccessViolationException();
        }

        [HttpPost]
        public async Task<bool> ChangeUserPassword(ChangeUserPasswordRequest request)
        {
            var loginResult = await _accountLogic.ValidateLogin(request, "Admin");
            if (loginResult.Success)
            {
                return await _accountLogic.ChangeUserPassword(request.UserEmail, request.NewPassword);
            }

            throw new AccessViolationException();
        }

        [HttpPost]
        public async Task<bool> ChangeUserRoles(ChangeUserRolesRequest request)
        {
            var loginResult = await _accountLogic.ValidateLogin(request, "Admin");
            if (loginResult.Success)
            {
                return await _accountLogic.ChangeUserRoles(request.UserEmail, request.Roles);
            }

            throw new AccessViolationException();
        }
    }
}