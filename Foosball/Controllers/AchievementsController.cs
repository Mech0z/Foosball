using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
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
        public async Task<AchievementsView> Index()
        {
            var activeSeason = await _seasonLogic.GetActiveSeason();

            var ach = await _achievementsService.GetAchievementsView(activeSeason.Name);

            return ach;
        }
    }
}
