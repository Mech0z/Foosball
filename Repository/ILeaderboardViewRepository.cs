using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface ILeaderboardViewRepository
    {
        LeaderboardView GetLeaderboardView(string seasonName);
        List<LeaderboardView> GetLeaderboardViews();
        Task Upsert(LeaderboardView view);
    }
}