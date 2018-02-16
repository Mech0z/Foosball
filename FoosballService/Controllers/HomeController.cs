using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoosballCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
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
            //var userManager = _provider.GetService<UserManager<IdentityUser>>();
            IdentityUser result = _userManager.Users.ToList().First();
            if (result.Roles.All(r => r != "Admin"))
            {
                result.AddRole("Admin");
                await _userStore.UpdateAsync(result, CancellationToken.None);
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
