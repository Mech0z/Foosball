using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class PlayerRanksController : Controller
    {
        private readonly IPlayerRankHistoryRepository _playerRankHistoryRepository;

        public PlayerRanksController(IPlayerRankHistoryRepository playerRankHistoryRepository)
        {
            _playerRankHistoryRepository = playerRankHistoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerRankAsync(string email)
        {
            var playerRankHistory = await _playerRankHistoryRepository.GetPlayerRankHistory(email);
            return Ok(playerRankHistory);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerRanksAsync()
        {
            var playerRankHistories = await _playerRankHistoryRepository.GetPlayerRankHistories();
            return Ok(playerRankHistories);
        }
    }
}
