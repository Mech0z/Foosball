using System.Collections.Generic;
using FoosballCore.OldLogic;
using FoosballCore.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Old;

namespace FoosballCore.Controllers
{
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