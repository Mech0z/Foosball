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
    public class PlayerRankHistoryRepository : BaseRepository<PlayerRankSeasonEntry>, IPlayerRankHistoryRepository
    {
        public PlayerRankHistoryRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "PlayerRankHistories")
        {
        }

        public async Task<PlayerRankSeasonEntry> GetPlayerRankHistory(string email, string seasonName)
        {
            var result = await Collection.AsQueryable()
                .SingleOrDefaultAsync(x => x.Email == email && x.SeasonName == seasonName);

            return result;
        }

        public async Task<List<PlayerRankSeasonEntry>> GetPlayerRankHistories(string seasonName)
        {
            var query = Collection.AsQueryable().Where(x => x.SeasonName == seasonName);

            return await query.ToListAsync(); ;
        }

        public async Task RemovePlayerHistoryFromSeason(string seasonName)
        {
            await Collection.DeleteManyAsync(
                Builders<PlayerRankSeasonEntry>.Filter.Eq(x => x.SeasonName, seasonName));
        }

        public async Task Upsert(PlayerRankSeasonEntry playerRankSeasonEntry)
        {
            if (playerRankSeasonEntry.Id == Guid.Empty)
            {
                await Collection.InsertOneAsync(playerRankSeasonEntry);
            }
            else
            {
                await Collection.ReplaceOneAsync(i => i.Id == playerRankSeasonEntry.Id, playerRankSeasonEntry);
            }
        }
    }
}
