using System.Collections.Generic;
using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("MyPolicy")]
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

        [HttpGet]
        public async Task<IActionResult> GetSeasonsAsync()
        {
            var seasons = await _seasonLogic.GetSeasons();
            return Ok(seasons);
        }
    }
}