using System;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using Models;

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
        public async Task<IActionResult> StartNewSeason(UpsertSeasonRequest request)
        {
            if (request.StartDate <= DateTime.Today)
            {
                throw new ArgumentException("Date must be in the future");
            }
            var seasonName = await _seasonLogic.StartNewSeason(request);

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