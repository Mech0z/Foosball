using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<List<LeaderboardView>> Index()
        {
                return await _leaderboardService.GetLatestLeaderboardViews();
        }
    }
}