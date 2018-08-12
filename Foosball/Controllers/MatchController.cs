using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class MatchController : Controller
    {
        private readonly ILeaderboardService _leaderboardService;
        private readonly ILeaderboardViewRepository _leaderboardViewRepository;
        private readonly ISeasonLogic _seasonLogic;
        private readonly IAccountLogic _accountLogic;
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchupResultRepository _matchupResultRepository;

        public MatchController(IMatchRepository matchRepository,
            IMatchupResultRepository matchupResultRepository,
            ILeaderboardService leaderboardService,
            ILeaderboardViewRepository leaderboardViewRepository,
            ISeasonLogic seasonLogic,
            IAccountLogic accountLogic)
        {
            _matchRepository = matchRepository;
            _matchupResultRepository = matchupResultRepository;
            _leaderboardService = leaderboardService;
            _leaderboardViewRepository = leaderboardViewRepository;
            _seasonLogic = seasonLogic;
            _accountLogic = accountLogic;
        }

        // GET: /api/Match/GetAll
        [HttpGet]
        public async Task<IEnumerable<Match>> GetAll()
        {
            try
            {
                var matches = await _matchRepository.GetMatches(null);

                return matches;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "{ExceptionSource} had an error", ex.Source);
                throw;
            }
        }

        // GET: /api/Match/LastGames?numberOfMatches=10
        [HttpGet]
        public async Task<IEnumerable<Match>> LastGames(int numberOfMatches)
        {
            return await _matchRepository.GetRecentMatches(numberOfMatches);
        }

        [HttpPost]
        [ClaimRequirement("Permission", ClaimRoles.Player)]
        public async Task<IActionResult> SaveMatch(SaveMatchesRequest saveMatchesRequest)
        {
            if (saveMatchesRequest == null)
            {
                return BadRequest();
            }

            var hasAdminClaim = HttpContext.User.HasClaim(x => x.Type == "Role" && x.Value == ClaimRoles.Admin.ToString());

            foreach (var match in saveMatchesRequest.Matches)
            {
                if (!match.PlayerList.Contains(saveMatchesRequest.Email) && !hasAdminClaim)
                {
                    return Unauthorized();
                }

                if (!ValidateScore(match))
                {
                    return Forbid($"Invalid match score {match.MatchResult.Team1Score} - {match.MatchResult.Team2Score}");
                }
            }
            
            var seasons = await _seasonLogic.GetSeasons();

            if (seasons.All(x => x.EndDate != null))
            {
                return BadRequest(); //"No active seaons"
            }

            var currentSeason = seasons.Single(x => x.EndDate == null);

            var isEdit = saveMatchesRequest.Matches.Any(x => x.Id == Guid.Empty);
            var fromPreviousSeason = saveMatchesRequest.Matches.Any(x => x.TimeStampUtc < currentSeason.StartDate);

            if (isEdit && fromPreviousSeason && !hasAdminClaim)
            {
                return Forbid();
            }

            var matches = saveMatchesRequest.Matches.OrderBy(x => x.TimeStampUtc).ToList();
            
            foreach (var match in matches)
            {
                match.SubmittedBy = saveMatchesRequest.Email;
                if (match.TimeStampUtc == DateTime.MinValue)
                {
                    match.TimeStampUtc = DateTime.UtcNow;
                }

                match.SeasonName = currentSeason.Name;

                var leaderboards = await _leaderboardService.GetLatestLeaderboardViews();

                var activeLeaderboard = leaderboards.SingleOrDefault(x => x.SeasonName == currentSeason.Name);

                if (!isEdit)
                {
                    _leaderboardService.AddMatchToLeaderboard(activeLeaderboard, match);
                }

                await _matchRepository.Upsert(match);

                if (!isEdit)
                {
                    await _leaderboardViewRepository.Upsert(activeLeaderboard);
                }
            }

            if (isEdit)
            {
                if (fromPreviousSeason)
                {
                    var season = seasons.Single(x =>
                        x.StartDate <= matches.First().TimeStampUtc && x.EndDate >= matches.First().TimeStampUtc);
                    await _leaderboardService.RecalculateLeaderboard(season.Name);
                }
                else
                {
                    await _leaderboardService.RecalculateLeaderboard(currentSeason.Name);
                }
            }

            return Ok();
        }

        private bool ValidateScore(Match match)
        {
            var team1Score = match.MatchResult.Team1Score;
            var team2Score = match.MatchResult.Team2Score;

            if (team1Score < 0 || team2Score < 0)
            {
                return false;
            }

            if (team1Score < 8 && team2Score < 8)
            {
                return false;
            }

            if ((team1Score == 8 && team2Score <= 6) || (team1Score <= 6 && team2Score == 8))
            {
                return true;
            }

            if (team1Score > 8)
            {
                return team2Score == team1Score + 2 || team2Score == team1Score - 2;
            }

            if (team2Score > 8)
            {
                return team1Score == team2Score + 2 || team1Score == team2Score - 2;
            }

            return true;
        }

        [HttpGet]
        public MatchupResult GetMatchupResult(List<string> userlist)
        {
            try
            {
                userlist = new List<string> { "jasper@sovs.net", "maso@seges.dk", "madsskipper@gmail.com", "anjaskott@gmail.com"};

                //Sort
                    var sortedUserlist = userlist.OrderBy(x => x).ToList();
                var addedList = string.Join("", sortedUserlist.ToArray());

                //RecalculateLeaderboard hashstring
                var hashcode = addedList.GetHashCode();

                //RecalculateLeaderboard the correct one
                var results = _matchupResultRepository.GetByHashResult(hashcode);

                //TODO dont seem optimal to create a list every time
                var team1list = new List<string> {userlist[0], userlist[1]};
                var team1Hashcode = team1list.OrderBy(x => x).GetHashCode();

                var team2list = new List<string> {userlist[3], userlist[4]};
                var team2Hashcode = team2list.OrderBy(x => x).GetHashCode();

                var result =
                    results.Single(x =>
                        (x.Team1HashCode == team1Hashcode || x.Team1HashCode == team2Hashcode) &&
                        (x.Team2HashCode == team1Hashcode || x.Team2HashCode == team2Hashcode));

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "{ExceptionSource} had an error", ex.Source);
                throw;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<Match>> TodaysMatches()
        {
            return await _matchRepository.GetMatchesByTimeStamp(DateTime.Today);
        }
    }
}