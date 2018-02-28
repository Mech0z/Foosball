using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Old;
using Repository;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;

namespace FoosballCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Administration")]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserRepository _userRepository;

        public AdministrationController(UserManager<Microsoft.AspNetCore.Identity.MongoDB.IdentityUser> userManager,
            IUserStore<IdentityUser> userStore, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _userRepository = userRepository;
        }

        public async IList<CombinedUser> GetUsers()
        {
            var combinedUsers = new List<CombinedUser>();

            var internalUsers = await _userRepository.GetUsers();
            var identityUsers = await _userManager.Users.ToListAsync();

            foreach (User internalUser in internalUsers)
            {
                
            }

            return combinedUsers;
        }
    }
}