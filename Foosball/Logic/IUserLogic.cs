using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;
using Models.RequestResponses;

namespace Foosball.Logic
{
    public interface IUserLogic
    {
        Task<List<PlayerLeaderboardEntry>> GetPlayerLeaderboardEntries(string email);
        Task<EggStats> GetEggStats(string email);
    }
}