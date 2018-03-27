using System.Linq;
using System.Threading.Tasks;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles= "Admin")]
    public class AdministrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public AdministrationController(
            IUserRepository userRepository,
            IIdentityUserRepository identityUserRepository)
        {
            _userRepository = userRepository;
            _identityUserRepository = identityUserRepository;
        }

        [HttpGet]
        public async Task<GetUserMappingsResponse> GetUserMappings()
        {
            var normalUsers = await _userRepository.GetUsersAsync();
            var identityUsernames = await _identityUserRepository.GetIdentityEmails();
            
            return new GetUserMappingsResponse
            {
                NormalUsernames = normalUsers.Select(x => x.Username).ToList(),
                IdentityEmails = identityUsernames 
            };
        }
    }
}