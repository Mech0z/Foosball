using System.Linq;
using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<AchievementsView> Index(string? seasonName)
        {
            var seasons = await _seasonLogic.GetSeasons();
            Season selectedSeason;
            if (seasonName != null)
            {
                selectedSeason = seasons.SingleOrDefault(x => x.Name == seasonName);
            }
            else
            {
                selectedSeason = await _seasonLogic.GetActiveSeason(seasons);
            }

            var ach = await _achievementsService.GetAchievementsView(seasons, selectedSeason);

            return ach;
        }
    }
}
