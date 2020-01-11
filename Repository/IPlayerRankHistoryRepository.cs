using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IPlayerRankHistoryRepository
    {
        Task<PlayerRankHistory> GetPlayerRankHistory(string email);
        Task<List<PlayerRankHistory>> GetPlayerRankHistories();
        Task Upsert(PlayerRankHistory playerRankHistory);
    }
}