using System.Collections.Generic;
using Models.Old;

namespace FoosballCore.OldLogic
{
    public interface ILeaderboardService : ILogic
    {
        LeaderboardView RecalculateLeaderboard(string season);
        LeaderboardView GetActiveLeaderboard();
        List<LeaderboardView> GetLatestLeaderboardViews();
        void AddMatchToLeaderboard(LeaderboardView leaderboardView, Match match);
    }
}