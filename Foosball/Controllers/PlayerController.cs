using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Logic;
using Foosball.Middleware;
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
        private readonly IAccountLogic _accountLogic;
        private readonly IUserLoginInfoRepository _userLoginInfoRepository;

        public PlayerController(IMatchRepository matchRepository,
            IUserRepository userRepository, 
            IMatchupHistoryCreator matchupHistoryCreator, 
            ISeasonLogic seasonLogic,
            IUserLogic userLogic,
            IAccountLogic accountLogic,
            IUserLoginInfoRepository userLoginInfoRepository
            )
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _matchupHistoryCreator = matchupHistoryCreator;
            _seasonLogic = seasonLogic;
            _userLogic = userLogic;
            _accountLogic = accountLogic;
            _userLoginInfoRepository = userLoginInfoRepository;
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
            var activeSeason = await _seasonLogic.GetActiveSeason();
            var seasons = await _seasonLogic.GetSeasons();
            return await _matchupHistoryCreator.GetPartnerWinPercent(seasons, email, activeSeason);
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
        public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Email == null) throw new ArgumentNullException(nameof(request.Email));
            if (request.Password == null) throw new ArgumentNullException(nameof(request.Password));
            if (request.Username == null) throw new ArgumentNullException(nameof(request.Username));

            if (!request.Email.Contains("@"))
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

            var users = await _userRepository.GetUsersAsync();

            if (users.Any(x => string.Equals(x.Email, request.Email, StringComparison.CurrentCultureIgnoreCase)))
            {
                return BadRequest("Email already taken");
            }

            var createResult = await _accountLogic.CreateUser(request.Email.ToLower(), request.Username, request.Password);

            if (!createResult)
                return BadRequest();

            return Ok("User created");
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Player)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Email == null) throw new ArgumentNullException(nameof(request.Email));
            if (request.NewPassword == null) throw new ArgumentNullException(nameof(request.NewPassword));

            var loginSession = HttpContext.GetLoginSession();

            if(request.Email != loginSession.Email)
            {
                return BadRequest("Email does not match login");
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest("Password too short");
            }

            await _userLoginInfoRepository.ChangePassword(request.Email, request.NewPassword);

            return Ok("Password changed");
        }

        [HttpGet]
        public async Task<IActionResult> RequestPassword([FromQuery] string email)
        {
            var requestResult = await _accountLogic.RequestPassword(email);

            if (requestResult)
                return Ok();

            return BadRequest();
        }
    }
}