using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoosballCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace FoosballCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceProvider _provider;

        public HomeController(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<IActionResult> Index()
        {
            var userManager = _provider.GetService<UserManager<IdentityUser>>();
            var result = await userManager.Users.ToListAsync();
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
