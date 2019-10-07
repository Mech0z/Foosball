using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface ILeaderboardViewRepository
    {
        Task<LeaderboardView> GetLeaderboardView(Season season);
        Task<List<LeaderboardView>> GetLeaderboardViews();
        Task Upsert(LeaderboardView view);
    }
}