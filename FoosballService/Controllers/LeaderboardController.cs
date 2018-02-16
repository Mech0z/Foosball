using System.Collections.Generic;
using FoosballCore.OldLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace FoosballCore.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        // GET: /<controller>/
        [HttpGet]
        public IEnumerable<LeaderboardView> Index()
        {
                return _leaderboardService.GetLatestLeaderboardViews();
        }
    }
}