using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface IPlayerRankLogic
    {
        Task<PlayerRankSeasonEntry> GetPlayerRankAsync(string email, string seasonName);
        Task<List<PlayerRankSeasonEntry>> GetPlayerRanksAsync(string seasonName);
    }
}