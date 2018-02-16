using FoosballCore.OldLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace FoosballCore.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly IAchievementsService _achievementsService;
        private readonly ISeasonLogic _seasonLogic;

        public AchievementsController(IAchievementsService achievementsService, ISeasonLogic seasonLogic)
        {
            _achievementsService = achievementsService;
            _seasonLogic = seasonLogic;
        }

        [HttpGet]
        public AchievementsView Index()
        {
            var activeSeason = _seasonLogic.GetActiveSeason();

            var ach = _achievementsService.GetAchievementsView(activeSeason.Name);

            return ach;
        }
    }
}
