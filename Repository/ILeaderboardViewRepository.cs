using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface ILeaderboardViewRepository
    {
        Task<LeaderboardView> GetLeaderboardView(string seasonName);
        Task<List<LeaderboardView>> GetLeaderboardViews();
        Task Upsert(LeaderboardView view);
    }
}