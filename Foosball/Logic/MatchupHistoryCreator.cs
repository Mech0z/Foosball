﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class MatchupHistoryCreator : IMatchupHistoryCreator
    {
        private readonly IMatchupResultRepository _matchupResultRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILeaderboardViewRepository _leaderboardViewRepository;

        public MatchupHistoryCreator(IMatchupResultRepository matchupResultRepository, IMatchRepository matchRepository, IUserRepository userRepository, ILeaderboardViewRepository leaderboardViewRepository)
        {
            _matchupResultRepository = matchupResultRepository;
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _leaderboardViewRepository = leaderboardViewRepository;
        }

        public async Task<List<PartnerPercentResult>> GetPartnerWinPercent(List<Season> seasons, string email,
            Season season)
        {
            var leaderboard = await _leaderboardViewRepository.GetLeaderboardView(season);
            double? normalWinRate = null;
            if (leaderboard != null)
            {
                // ReSharper disable once RedundantCast
                normalWinRate = leaderboard.Entries.Where(e => e.UserName == email).Select(e => Math.Round((((double)e.Wins / (double)e.NumberOfGames) * 100),2)).FirstOrDefault();
            }

            var result = new List<PartnerPercentResult>();

            foreach (User user in await _userRepository.GetUsersAsync())
            {
                if (user.Email != email)
                {
                    result.Add(new PartnerPercentResult
                    {
                        Username = user.Username,
                        Email = user.Email,
                        UsersNormalWinrate = normalWinRate
                    });
                }
            }

            var matches = await _matchRepository.GetMatches(season.StartDate, HelperMethods.GetNextSeason(seasons, season)?.StartDate);

            foreach (Match match in matches)
            {
                ManipulatePartnerResults(email, match, result, 0, 1, 2, 3);
                ManipulatePartnerResults(email, match, result, 1, 0, 2, 3);
                ManipulatePartnerResults(email, match, result, 2, 3, 0, 1);
                ManipulatePartnerResults(email, match, result, 3, 2, 0, 1);
            }

            return result
                .Where(x => x.MatchesTogether > 0 || x.MatchesAgainst > 0)
                .OrderByDescending(x => (double)x.WinsTogether / (double)x.MatchesTogether)
                .ToList();
        }

        private static void ManipulatePartnerResults(string email, Match match, List<PartnerPercentResult> result, int playerIndex, int partnerIndex, int enemy1Index, int enemy2Index)
        {
            if (match.PlayerList[playerIndex] == email)
            {
                PartnerPercentResult partner = result.Single(x => x.Email == match.PlayerList[partnerIndex]);
                partner.MatchesTogether++;
                PartnerPercentResult enemy1 = result.Single(x => x.Email == match.PlayerList[enemy1Index]);
                enemy1.MatchesAgainst++;
                PartnerPercentResult enemy2 = result.Single(x => x.Email == match.PlayerList[enemy2Index]);
                enemy2.MatchesAgainst++;
                
                if ((playerIndex == 0 || playerIndex == 1) && match.MatchResult.Team1Won)
                {
                    partner.WinsTogether++;
                    enemy1.WinsAgainst++;
                    enemy2.WinsAgainst++;
                }

                if ((playerIndex == 2 || playerIndex == 3) && match.MatchResult.Team1Won == false)
                {
                    partner.WinsTogether++;
                    enemy1.WinsAgainst++;
                    enemy2.WinsAgainst++;
                }
            }
        }

        public async Task AddMatch(Match match)
        {
            //Sort
            var sortedUserlist = match.PlayerList.OrderBy(x => x).ToList();
            var addedList = string.Join("", sortedUserlist.ToArray());

            //RecalculateLeaderboard hashstring
            var hashcode = addedList.GetHashCode();

            //Find existing matchup historys that have the same players
            List<MatchupResult> matchingResults = _matchupResultRepository.GetByHashResult(hashcode);

            //Find the team constalation with same team setup
            var team1 = new List<string> { match.PlayerList[0], match.PlayerList[1] }.OrderBy(x => x).ToList();
            var team2 = new List<string> { match.PlayerList[2], match.PlayerList[3] }.OrderBy(x => x).ToList();

            MatchupResult correctMatchupResult = null;
            foreach (var matchResult in matchingResults)
            {
                var sortedFoundMatchResultTeam1 = new List<string> { matchResult.UserList[0], matchResult.UserList[1] }.OrderBy(x => x).ToList();
                var sortedFoundMatchResultTeam2 = new List<string> { matchResult.UserList[2], matchResult.UserList[3] }.OrderBy(x => x).ToList();

                if (team1[0] == sortedFoundMatchResultTeam1[0] && team1[1] == sortedFoundMatchResultTeam1[1])
                {
                    correctMatchupResult = matchResult;
                    break;
                } else if (team2[0] == sortedFoundMatchResultTeam2[0] && team2[1] == sortedFoundMatchResultTeam2[1])
                {
                    correctMatchupResult = matchResult;
                    break;
                }
            }
            
            //Add match
            if (correctMatchupResult != null)
            {
                AddMatchupResult(correctMatchupResult, match);
                await _matchupResultRepository.Upsert(correctMatchupResult);
            }
            else
            {
                //Create new matchcupresult
                var matchup = new MatchupResult
                {
                    HashResult = hashcode,
                    Last5Games = new List<MatchResult> {match.MatchResult},
                    Team1Wins = 0,
                    Team2Wins = 0,
                    UserList = match.PlayerList
                };
                await _matchupResultRepository.Upsert(matchup);
            }
        }

        public void RecalculateMatchupHistory()
        {
            throw new System.NotImplementedException();
        }

        public void AddMatchupResult(MatchupResult existingMatchupResult, Match match)
        {
            //Find out if team 1
            if (match.PlayerList[0] == existingMatchupResult.UserList[0] ||
                match.PlayerList[0] == existingMatchupResult.UserList[1])
            {
                if (match.MatchResult.Team1Won)
                {
                    existingMatchupResult.Team1Wins++;
                }
                else
                {
                    existingMatchupResult.Team2Wins++;
                }
            }
            else
            {
                if (match.MatchResult.Team1Won)
                {
                    existingMatchupResult.Team2Wins++;
                }
                else
                {
                    existingMatchupResult.Team1Wins++;
                }
            }

            if (existingMatchupResult.Last5Games.Count < 5)
            {
                //TODO Need to take care when team 1 of existing is not the same as team1 in match
                existingMatchupResult.Last5Games.Insert(0, match.MatchResult);
            }
            else
            {
                existingMatchupResult.Last5Games.RemoveAt(4);
                existingMatchupResult.Last5Games.Insert(0, match.MatchResult);
            }
        }
    }
}