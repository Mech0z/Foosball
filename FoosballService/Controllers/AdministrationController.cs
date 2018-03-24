using System.Linq;
using System.Threading.Tasks;
using FoosballCore.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace FoosballCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles= "Admin")]
    public class AdministrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMappingRepository _userMappingRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public AdministrationController(
            IUserRepository userRepository,
            IUserMappingRepository userMappingRepository,
            IIdentityUserRepository identityUserRepository)
        {
            _userRepository = userRepository;
            _userMappingRepository = userMappingRepository;
            _identityUserRepository = identityUserRepository;
        }

        [HttpGet]
        public async Task<GetUserMappingsResponse> GetUserMappings()
        {
            var normalUsers = await _userRepository.GetUsersAsync();
            var userMappings = await _userMappingRepository.GetUserMappings();
            var identityUsernames = await _identityUserRepository.GetIdentityEmails();
            
            return new GetUserMappingsResponse
            {
                NormalUsernames = normalUsers.Select(x => x.Username).ToList(),
                UserMappings = userMappings,
                IdentityEmails = identityUsernames 
            };
        }
    }
}