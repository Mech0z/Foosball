using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ClaimRequirement("Permission", ClaimRoles.Admin)]
    [EnableCors("MyPolicy")]
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
            return await _accountLogic.ChangeUserPassword(request.UserEmail, request.NewPassword);
        }

        [HttpPost]
        public async Task<bool> ChangeUserRoles(ChangeUserRolesRequest request)
        {
            return await _accountLogic.ChangeUserRoles(request.UserEmail, request.Roles);
        }
    }
}