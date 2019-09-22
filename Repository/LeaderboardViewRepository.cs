using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public class LeaderboardViewRepository : BaseRepository<LeaderboardView>, ILeaderboardViewRepository
    {
        public LeaderboardViewRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "LeaderboardViews")
        {

        }

        public async Task<LeaderboardView> GetLeaderboardView(string seasonName)
        {
            var result = await Collection.AsQueryable()
                .Where(x => x.SeasonName == seasonName)
                .OrderBy(x => x.Timestamp)
                .ToListAsync();

            //query.OrderBy(x => x.Timestamp);
            //query.Where(x => x.SeasonName == seasonName);
            //var result = await query.ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<LeaderboardView>> GetLeaderboardViews()
        {
            var query = Collection.AsQueryable();

            return await query.ToListAsync();;
        }

        public async Task Upsert(LeaderboardView view)
        {
            if (view.Id == Guid.Empty)
            {
                await Collection.InsertOneAsync(view);
            }
            else
            {
                await Collection.ReplaceOneAsync(i => i.Id == view.Id, view);
            }
        }
    }
}