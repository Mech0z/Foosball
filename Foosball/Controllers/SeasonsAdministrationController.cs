using System.Collections.Generic;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SeasonsAdministrationController : Controller
    {
        private readonly ISeasonLogic _seasonLogic;

        public SeasonsAdministrationController(ISeasonLogic seasonLogic)
        {
            _seasonLogic = seasonLogic;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult StartNewSeason(VoidRequest request)
        {
            var seasonName = _seasonLogic.StartNewSeason();

            return Ok(seasonName);
        }

        [HttpPost]
        public IActionResult GetSeasons(VoidRequest request)
        {
            List<Season> seasons = _seasonLogic.GetSeasons();
            return Ok(seasons);
        }
    }
}