using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface ILeaderboardService : ILogic
    {
        Task<LeaderboardView> RecalculateLeaderboard(string season);
        LeaderboardView GetActiveLeaderboard();
        Task<List<LeaderboardView>> GetLatestLeaderboardViews();
        bool AddMatchToLeaderboard(LeaderboardView leaderboardView, Match match);
    }
}