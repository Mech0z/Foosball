using System.Collections.Generic;
using Models.Old;

namespace Repository
{
    public interface ILeaderboardViewRepository
    {
        LeaderboardView GetLeaderboardView(string seasonName);
        List<LeaderboardView> GetLeaderboardViews();
        void Upsert(LeaderboardView view);
    }
}