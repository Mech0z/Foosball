using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
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
        private readonly IAccountLogic _accountLogic;

        public LeaderboardController(ILeaderboardService leaderboardService,
            ISeasonLogic seasonLogic,
            IAccountLogic accountLogic)
        {
            _leaderboardService = leaderboardService;
            _seasonLogic = seasonLogic;
            _accountLogic = accountLogic;
        }

        [HttpGet]
        public async Task<List<LeaderboardView>> Index()
        {
            return await _leaderboardService.GetLatestLeaderboardViews();
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Admin)]
        public async Task ResetLeaderboard()
        {
            //var loginResult = await _accountLogic.ValidateLogin(request, "Admin");
            //if (!loginResult.Success) throw new AccessViolationException();

            var seasons = _seasonLogic.GetSeasons();
            foreach (var season in seasons)
            {
                await _leaderboardService.RecalculateLeaderboard(season.Name);
            }
        }
    }
}