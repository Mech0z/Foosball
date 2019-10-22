using System;
using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestResponses;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ClaimRequirement("Permission", ClaimRoles.Admin)]
    [ApiController]
    public class AdministrationController : Controller
    {
        private readonly IAccountLogic _accountLogic;

        public AdministrationController(IAccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        [HttpGet]
        public async Task<GetUserMappingsResponse> GetUserMappings()
        {
            return await _accountLogic.GetUserMappings();
        }

        [HttpPost]
        public async Task<bool> ChangeUserPassword(ChangeUserPasswordRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.NewPassword == null) throw new ArgumentNullException(nameof(request.NewPassword));
            if (request.UserEmail == null) throw new ArgumentNullException(nameof(request.UserEmail));

            return await _accountLogic.ChangeUserPassword(request.UserEmail, request.NewPassword);
        }

        [HttpPost]
        public async Task<bool> ChangeUserRoles(ChangeUserRolesRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Roles == null) throw new ArgumentNullException(nameof(request.Roles));
            if (request.UserEmail == null) throw new ArgumentNullException(nameof(request.UserEmail));

            return await _accountLogic.ChangeUserRoles(request.UserEmail, request.Roles);
        }
    }
}