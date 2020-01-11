using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class PlayerRankHistoryRepository : BaseRepository<PlayerRankHistory>, IPlayerRankHistoryRepository
    {
        public PlayerRankHistoryRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "PlayerRankHistories")
        {
        }

        public async Task<PlayerRankHistory> GetPlayerRankHistory(string email)
        {
            var result = await Collection.AsQueryable()
                .Where(x => x.Email == email)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<PlayerRankHistory>> GetPlayerRankHistories()
        {
            var query = Collection.AsQueryable();

            return await query.ToListAsync(); ;
        }

        public async Task Upsert(PlayerRankHistory playerRankHistory)
        {
            if (playerRankHistory.Id == Guid.Empty)
            {
                await Collection.InsertOneAsync(playerRankHistory);
            }
            else
            {
                await Collection.ReplaceOneAsync(i => i.Id == playerRankHistory.Id, playerRankHistory);
            }
        }
    }
}
