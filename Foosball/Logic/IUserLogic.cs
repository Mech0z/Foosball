using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface IUserLogic
    {
        Task<List<PlayerLeaderboardEntry>> GetPlayerLeaderboardEntries(string email);
    }
}