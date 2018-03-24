using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoosballCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;

namespace FoosballCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;

        public HomeController(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;
        }
        
        public async Task<IActionResult> Index()
        {
            var result = _userManager.Users.ToList();
            foreach (var user in result)
            {
                if (user.Roles.All(r => r != "Admin"))
                {
                    user.AddRole("Admin");
                    await _userStore.UpdateAsync(user, CancellationToken.None);
                }
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
