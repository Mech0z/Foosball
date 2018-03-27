using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballCore.OldLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Old;
using Repository;

namespace FoosballCore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMatchupHistoryCreator _matchupHistoryCreator;
        private readonly ISeasonLogic _seasonLogic;

        public PlayerController(IMatchRepository matchRepository,
            IUserRepository userRepository, 
            IMatchupHistoryCreator matchupHistoryCreator, 
            ISeasonLogic seasonLogic)
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _matchupHistoryCreator = matchupHistoryCreator;
            _seasonLogic = seasonLogic;
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
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }

        //[HttpPost]
        //public IActionResult CreateUser(User user)
        //{
        //    var users = _userRepository.GetUsers();

        //    if (string.IsNullOrEmpty(user.Email))
        //    {
        //        return BadRequest("Email cant be null");
        //    }

        //    if (users.Any(x => x.Email == user.Email))
        //    {
        //        return BadRequest();
        //    }

        //    _userRepository.AddUser(user);
        //    return Ok();
        //}

        //[HttpPost]
        //public IActionResult Login(User user)
        //{
        //    var hash = _userRepository.Login(user);
        //    if (hash == string.Empty)
        //    {
        //        return Unauthorized();
        //    }
            
        //    return Ok(hash);
        //}

        //[HttpPost]
        //public IActionResult ChangePassword(ChangePasswordRequest request)
        //{
        //    if (request.NewPassword.Length < 6)
        //    {
        //        return BadRequest("Password too short");
        //    }

        //    var hash = _userRepository.ChangePassword(request.Email, request.OldPassword, request.NewPassword);
            
        //    if (hash == string.Empty)
        //    {
        //        return Unauthorized();
        //    }
            
        //    return Ok(hash);
        //}
    }
}