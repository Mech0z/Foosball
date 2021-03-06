﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ILeaderboardViewRepository _leaderboardViewRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly ISeasonLogic _seasonLogic;
        private readonly IPlayerRankHistoryRepository _playerRankHistoryRepository;

        public LeaderboardService(ILeaderboardViewRepository leaderboardViewRepository,
            IMatchRepository matchRepository,
            ISeasonLogic seasonLogic,
            IPlayerRankHistoryRepository playerRankHistoryRepository)
        {
            _leaderboardViewRepository = leaderboardViewRepository;
            _matchRepository = matchRepository;
            _seasonLogic = seasonLogic;
            _playerRankHistoryRepository = playerRankHistoryRepository;
        }

        public async Task<LeaderboardView> RecalculateLeaderboard(Season season)
        {
            var seasons = await _seasonLogic.GetSeasons();
            
            await _playerRankHistoryRepository.RemovePlayerHistoryFromSeason(season.Name);
            var playerRankHistories = await _playerRankHistoryRepository.GetPlayerRankHistories(season.Name);

            var matches =
                (await _matchRepository.GetMatches(season.StartDate,
                    HelperMethods.GetNextSeason(seasons, season)?.StartDate))
                .OrderBy(x => x.TimeStampUtc);

            var existingLeaderboard = await _leaderboardViewRepository.GetLeaderboardView(season);
            
            var leaderboardView = existingLeaderboard ?? new LeaderboardView(season.Name);
            leaderboardView.Entries = new List<LeaderboardViewEntry>(); //Clear old matches if existing leaderboard exists

            foreach (var match in matches)
            { 
                var matchPointsChanged = AddMatchToLeaderboard(leaderboardView, match);
                UpdatePlayerRanks(playerRankHistories,
                    leaderboardView.Entries.OrderByDescending(x => x.EloRating).ToList(), season.Name,
                    match.TimeStampUtc);
                if (matchPointsChanged)
                {
                    await _matchRepository.Upsert(match);
                }
            }
            leaderboardView.Entries = leaderboardView.Entries.OrderByDescending(x => x.EloRating).ToList();

            await _leaderboardViewRepository.Upsert(leaderboardView);

            //Only update those who played this season
            foreach (PlayerRankSeasonEntry playerRankHistory in playerRankHistories.Where(x =>
                leaderboardView.Entries.Select(x => x.UserName).Contains(x.Email)))
            {
                await _playerRankHistoryRepository.Upsert(playerRankHistory);
            }
            
            return leaderboardView;
        }

        public List<PlayerRankSeasonEntry> UpdatePlayerRanks(
            List<PlayerRankSeasonEntry> playerRankHistories,
            List<LeaderboardViewEntry> entries,
            string seasonName,
            DateTime matchDate)
        {
            foreach (LeaderboardViewEntry entry in entries)
            {
                int rank = entries.IndexOf(entry) + 1;
                PlayerRankSeasonEntry playerRankHistory =
                    playerRankHistories.SingleOrDefault(x => x.Email == entry.UserName);

                //Player has no history data
                if (playerRankHistory == null)
                {
                    playerRankHistory = new PlayerRankSeasonEntry(entry.UserName, seasonName);
                    playerRankHistories.Add(playerRankHistory);
                }

                var lastEntry = playerRankHistory.RankPlots.OrderBy(x => x.Date).LastOrDefault();
                if (lastEntry != null && lastEntry.EloRating == entry.EloRating && lastEntry.Rank == rank)
                {
                    continue;
                }
                playerRankHistory.RankPlots.Add(new PlayerRankPlot(matchDate, rank, entry.EloRating));
            }

            return playerRankHistories;
        }

        public async Task<List<LeaderboardView>> GetLatestLeaderboardViews()
        {
            var seasons = await _seasonLogic.GetStartedSeasons();

            var latestLeaderboardViews = await _leaderboardViewRepository.GetLeaderboardViews();

            foreach (Season season in seasons)
            {
                bool existingLeaderboard = latestLeaderboardViews.Any(x => x.SeasonName == season.Name);

                if (existingLeaderboard == false)
                {
                    var leaderboardView = await RecalculateLeaderboard(season);
                    if (leaderboardView.Entries != null)
                    {
                        latestLeaderboardViews.Add(leaderboardView);
                    }
                }
            }

            foreach (LeaderboardView leaderboardView in latestLeaderboardViews)
            {
                leaderboardView.Entries = leaderboardView.Entries.OrderByDescending(x => x.EloRating).ToList();
            }

            List<LeaderboardView> startDateSorted = latestLeaderboardViews
                .Select(x => new {Lbw = x, Season = seasons.Single(s => s.Name == x.SeasonName)})
                .OrderByDescending(x => x.Season.StartDate).Select(x => x.Lbw).ToList();

            return startDateSorted;
        }

        public bool AddMatchToLeaderboard(LeaderboardView leaderboardView, Match match)
        {
            var existingPointsOnMatch = match.Points;
            var leaderboardEntries = leaderboardView.Entries;

            //Team1
            var player1 = match.PlayerList[0];
            var existingPlayer1 = leaderboardEntries.SingleOrDefault(x => x.UserName == player1);
            var player2 = match.PlayerList[1];
            var existingPlayer2 = leaderboardEntries.SingleOrDefault(x => x.UserName == player2);

            var team1AvgElo = existingPlayer1?.EloRating ?? 1500;
            team1AvgElo += existingPlayer2?.EloRating ?? 1500;

            //Team2
            var player3 = match.PlayerList[2];
            var existingPlayer3 = leaderboardEntries.SingleOrDefault(x => x.UserName == player3);
            var player4 = match.PlayerList[3];
            var existingPlayer4 = leaderboardEntries.SingleOrDefault(x => x.UserName == player4);

            var team2AvgElo = existingPlayer3?.EloRating ?? 1500;
            team2AvgElo += existingPlayer4?.EloRating ?? 1500;

            var elo = new EloRating();
            var result = elo.CalculateRating(team1AvgElo/2, team2AvgElo/2, match.MatchResult.Team1Won);

            match.Points = (int) result;

            if (existingPlayer1 == null)
            {
                leaderboardEntries.Add(CreatePlayer(player1, match, result, match.MatchResult.Team1Won));
            }
            else
            {
                UpdateExistingLeaderboardEntry(existingPlayer1.UserName, leaderboardEntries, match, result,
                    match.MatchResult.Team1Won);
            }

            if (existingPlayer2 == null)
            {
                leaderboardEntries.Add(CreatePlayer(player2, match, result, match.MatchResult.Team1Won));
            }
            else
            {
                UpdateExistingLeaderboardEntry(existingPlayer2.UserName, leaderboardEntries, match, result,
                    match.MatchResult.Team1Won);
            }

            if (existingPlayer3 == null)
            {
                leaderboardEntries.Add(CreatePlayer(player3, match, result, !match.MatchResult.Team1Won));
            }
            else
            {
                UpdateExistingLeaderboardEntry(existingPlayer3.UserName, leaderboardEntries, match, result,
                    !match.MatchResult.Team1Won);
            }

            if (existingPlayer4 == null)
            {
                leaderboardEntries.Add(CreatePlayer(player4, match, result, !match.MatchResult.Team1Won));
            }
            else
            {
                UpdateExistingLeaderboardEntry(existingPlayer4.UserName, leaderboardEntries, match, result,
                    !match.MatchResult.Team1Won);
            }

            return existingPointsOnMatch != match.Points;
        }

        public LeaderboardViewEntry CreatePlayer(string playerName, Match match, double result, bool won)
        {
            return new LeaderboardViewEntry
            {
                UserName = playerName,
                EloRating = won ? 1500 + (int) result : 1500 - (int) result,
                NumberOfGames = 1,
                Wins = won ? 1 : 0,
                Losses = won ? 0 : 1,
                Form = won ? "W" : "L"
            };
        }

        public void UpdateExistingLeaderboardEntry(string playerName, List<LeaderboardViewEntry> leaderboardEntries,
            Match match, double result, bool won)
        {
            var playerEntry = leaderboardEntries.Single(x => x.UserName == playerName);
            playerEntry.EloRating += won ? (int) result : -(int) result;
            playerEntry.NumberOfGames++;

            if (playerEntry.Form.Length < 5)
            {
                playerEntry.Form += won ? "W" : "L";
            }
            else
            {
                playerEntry.Form = playerEntry.Form.Remove(0, 1);
                playerEntry.Form += won ? "W" : "L";
            }

            if (won)
            {
                playerEntry.Wins++;
            }
            else
            {
                playerEntry.Losses++;
            }
        }
    }
}