using System.Threading.Tasks;
using Foosball.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class PlayerRanksController : Controller
    {
        private readonly IPlayerRankLogic _playerRankLogic;

        public PlayerRanksController(IPlayerRankLogic playerRankLogic)
        {
            _playerRankLogic = playerRankLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerRankAsync(string email, string seasonName)
        {
            var playerRankHistory = await _playerRankLogic.GetPlayerRankAsync(email, seasonName);
            return Ok(playerRankHistory);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerRanksAsync(string seasonName)
        {
            var playerRankHistories = await _playerRankLogic.GetPlayerRanksAsync(seasonName);
            return Ok(playerRankHistories);
        }
    }
}
