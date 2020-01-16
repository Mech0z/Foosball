using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IPlayerRankHistoryRepository
    {
        Task<PlayerRankSeasonEntry> GetPlayerRankHistory(string email, string seasonName);
        Task<List<PlayerRankSeasonEntry>> GetPlayerRankHistories(string seasonName);
        Task Upsert(PlayerRankSeasonEntry playerRankEntry);
        Task RemovePlayerHistoryFromSeason(string seasonName);
    }
}