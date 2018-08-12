using System.Collections.Generic;
using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Old;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ISeasonLogic _seasonLogic;

        public LeaderboardController(ILeaderboardService leaderboardService,
            ISeasonLogic seasonLogic,
            IAccountLogic accountLogic)
        {
            _leaderboardService = leaderboardService;
            _seasonLogic = seasonLogic;
        }

        [HttpGet]
        public async Task<List<LeaderboardView>> Index()
        {
            return await _leaderboardService.GetLatestLeaderboardViews();
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Admin)]
        public async Task<IActionResult> ResetLeaderboard()
        {
            var seasons = await _seasonLogic.GetSeasons();
            foreach (var season in seasons)
            {
                await _leaderboardService.RecalculateLeaderboard(season.Name);
            }

            return Ok();
        }
    }
}