using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Old;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class LeaderboardController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ISeasonLogic _seasonLogic;

        public LeaderboardController(ILeaderboardService leaderboardService,
            ISeasonLogic seasonLogic)
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
        public async Task<IActionResult> ResetLeaderboards()
        {
            var seasons = await _seasonLogic.GetSeasons();
            foreach (var season in seasons)
            {
                await _leaderboardService.RecalculateLeaderboard(season.Name);
            }

            return Ok();
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Admin)]
        public async Task<IActionResult> ResetLeaderboard(string seasonName)
        {
            var seasons = await _seasonLogic.GetSeasons();

            var season = seasons.SingleOrDefault(x => x.Name == seasonName);

            if (season != null)
            {
                await _leaderboardService.RecalculateLeaderboard(season.Name);
                return Ok();
            }

            return BadRequest();
            
        }
    }
}