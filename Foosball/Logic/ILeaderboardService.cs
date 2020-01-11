using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface ILeaderboardService : ILogic
    {
        Task<LeaderboardView> RecalculateLeaderboard(Season season);
        Task<List<LeaderboardView>> GetLatestLeaderboardViews();
        bool AddMatchToLeaderboard(LeaderboardView leaderboardView, Match match);
        List<PlayerRankHistory> UpdatePlayerRanks(
            List<PlayerRankHistory> playerRankHistories,
            List<LeaderboardViewEntry> entries,
            string seasonName,
            DateTime matchDate);
    }
}