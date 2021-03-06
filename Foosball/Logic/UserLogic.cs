﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Old;
using Models.RequestResponses;
using Repository;

namespace Foosball.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly ILeaderboardViewRepository _leaderboardViewRepository;
        private readonly IMatchRepository _matchRepository;

        public UserLogic(ILeaderboardViewRepository leaderboardViewRepository, IMatchRepository matchRepository)
        {
            _leaderboardViewRepository = leaderboardViewRepository;
            _matchRepository = matchRepository;
        }

        public async Task<EggStats> GetEggStats(string email)
        {
            var result = new EggStats();
            var userMatches = await _matchRepository.GetPlayerMatches(email);

            foreach (Match match in userMatches)
            {
                var indexOf = match.PlayerList.IndexOf(email);
                var playerIsOnTeam1 = indexOf == 0 || indexOf == 1;

                var team1Score = match.MatchResult.Team1Score;
                var team2Score = match.MatchResult.Team2Score;
                switch (team1Score)
                {
                    case 0 when playerIsOnTeam1:
                        result.MatchesReceivedEgg.Add(match);
                        break;
                    case 0:
                        result.MatchesGivenEgg.Add(match);
                        break;
                }

                if (team2Score != 0) continue;
                if (playerIsOnTeam1)
                {
                    result.MatchesGivenEgg.Add(match);
                }
                else
                {
                    result.MatchesReceivedEgg.Add(match);
                }
            }

            return result;
        }

        public async Task<List<PlayerLeaderboardEntry>> GetPlayerLeaderboardEntries(string email)
        {
            var result = new List<PlayerLeaderboardEntry>();
            var leaderboards = await _leaderboardViewRepository.GetLeaderboardViews();

            foreach (var leaderboard in leaderboards)
            {
                var leaderboardEntries = leaderboard.Entries.OrderByDescending(x => x.EloRating).ToList();
                var playerEntry = leaderboardEntries.SingleOrDefault(x => x.UserName == email);
                if (playerEntry != null)
                {
                    result.Add(new PlayerLeaderboardEntry
                    {
                        UserName = email,
                        EloRating = playerEntry.EloRating,
                        Losses = playerEntry.Losses,
                        NumberOfGames = playerEntry.NumberOfGames,
                        Wins = playerEntry.Wins,
                        SeasonName = leaderboard.SeasonName,
                        Rank = leaderboardEntries.IndexOf(playerEntry) + 1
                    });
                }
            }

            return result;
        }
    }
}