using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<PlayerRankHistoryPlot>> GetPlayerRankEntries(string email, string seasonName)
        {
            //r test = Collection.Find(Builders<PlayerRankHistory>.Filter.Eq(x => x.Email, email)).ToCursor().Current.FirstOrDefault(x => x.)
            
                

            //var sw = new Stopwatch();
            //sw.Start();
            ////var data = await Collection.Find(Builders<PlayerRankHistory>.Filter.Eq(x => x.Email, email))
            ////    .Select(y => y.PlayerRankHistorySeasonEntries.Where(z => z.SeasonName.Equals(seasonName)))
            ////    .FirstOrDefaultAsync()?.HistoryPlots
            ////    .ToList();
            //var elaped = sw.ElapsedMilliseconds;
            return data;
        }

        public async Task<PlayerRankHistory> GetPlayerRankHistory(string email)
        {
            var result = await Collection.AsQueryable()
                .Where(x => x.Email == email)
                .SingleOrDefaultAsync();
            return result;
        }

        public async Task<List<PlayerRankHistory>> GetPlayerRankHistories()
        {
            var query = Collection.AsQueryable();

            return await query.ToListAsync(); ;
        }

        public async Task RemovePlayerHistoryFromSeason(string seasonName)
        {
            var list = await GetPlayerRankHistories();
            foreach (PlayerRankHistory history in list)
            {
                var current = history.PlayerRankHistorySeasonEntries.SingleOrDefault(x => x.SeasonName == seasonName);
                history.PlayerRankHistorySeasonEntries.Remove(current);
                await Upsert(history);
            }
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
