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
    public class SeasonsAdministrationController : Controller
    {
        private readonly ISeasonLogic _seasonLogic;

        public SeasonsAdministrationController(ISeasonLogic seasonLogic)
        {
            _seasonLogic = seasonLogic;
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Admin)]
        public IActionResult StartNewSeason()
        {
            var seasonName = _seasonLogic.StartNewSeason();

            return Ok(seasonName);
        }

        [HttpPost]
        public async Task<IActionResult> GetSeasons()
        {
            List<Season> seasons = await _seasonLogic.GetSeasons();
            return Ok(seasons);
        }
    }
}