using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Old;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMatchupHistoryCreator _matchupHistoryCreator;
        private readonly ISeasonLogic _seasonLogic;
        private readonly IUserLogic _userLogic;

        public PlayerController(IMatchRepository matchRepository,
            IUserRepository userRepository, 
            IMatchupHistoryCreator matchupHistoryCreator, 
            ISeasonLogic seasonLogic,
            IUserLogic userLogic)
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _matchupHistoryCreator = matchupHistoryCreator;
            _seasonLogic = seasonLogic;
            _userLogic = userLogic;
        }

        [HttpGet]
        public async Task<List<Match>> GetPlayerMatches(string email)
        {
            var matches = await _matchRepository.GetPlayerMatches(email);
            return matches.OrderByDescending(x => x.TimeStampUtc).ToList();
        }

        [HttpGet]
        public async Task<List<PartnerPercentResult>> GetPlayerPartnerResults(string email)
        {
            var activeSeason = _seasonLogic.GetActiveSeason();

            return await _matchupHistoryCreator.GetPartnerWinPercent(email, activeSeason.Name);
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Select(user => new UserResponse(user));
        }

        [HttpGet]
        public async Task<GetPlayerSeasonHistoryResponse> GetPlayerHistory(string email)
        {
            var responseData = await _userLogic.GetPlayerLeaderboardEntries(email);
            var eggData = await _userLogic.GetEggStats(email);
            return new GetPlayerSeasonHistoryResponse
            {
                PlayerLeaderBoardEntries = responseData,
                EggStats = eggData
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var users = await _userRepository.GetUsersAsync();

            if (string.IsNullOrEmpty(request.Email) || !request.Email.Contains("@"))
            {
                return BadRequest("Invalid email");
            }

            if (request.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long");
            }

            if (request.Username.Length < 6)
            {
                return BadRequest("Username must be at least 6 characters long");
            }

            if (users.Any(x => x.Email == request.Email))
            {
                return BadRequest("Email already taken");
            }

            await _userRepository.AddUser(request.Email, request.Username, request.Password);
            return Ok("User created");
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Player)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (request.NewPassword.Length < 6)
            {
                return BadRequest("Password too short");
            }

            await _userRepository.ChangePassword(request.Email, request.NewPassword);

            return Ok("Password changed");
        }
    }
}