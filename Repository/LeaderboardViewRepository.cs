using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using Models.Old;

namespace Repository
{
    public class LeaderboardViewRepository : BaseRepository<LeaderboardView>, ILeaderboardViewRepository
    {
        public LeaderboardViewRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "LeaderboardViews")
        {

        }

        public LeaderboardView GetLeaderboardView(string seasonName)
        {
            var query = Collection.AsQueryable();

            query.OrderBy(x => x.Timestamp);
            query.Where(x => x.SeasonName == seasonName);
            var result = query.ToList().Where(x => x.SeasonName == seasonName).ToList();

            return result.FirstOrDefault();
        }

        public List<LeaderboardView> GetLeaderboardViews()
        {
            var query = Collection.AsQueryable();

            query.OrderBy(x => x.Timestamp);
            
            return query.ToList();
        }

        public void Upsert(LeaderboardView view)
        {
            Collection.ReplaceOne(i => i.Id == view.Id, view,
                            new UpdateOptions { IsUpsert = true });
        }
    }
}