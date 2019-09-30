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
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "Activity")
        {
        }

        public async Task UpsertActivity(Activity activity)
        {
            var exists = await GetActivityByDate(DateTime.UtcNow.Date);
            if (exists == null)
            {
                await Collection.InsertOneAsync(activity);
            }
            else
            {
                activity.Id = exists.Id;
                await Collection.ReplaceOneAsync(x => x.Date == activity.Date, activity);
            }
        }

        public async Task<Activity> GetActivityByDate(DateTime date)
        {
            var queryable = Collection.AsQueryable().Where(x => x.Date == date);
            return await queryable.SingleOrDefaultAsync();
        }

        public async Task<List<Activity>> GetActivities()
        {
            return await Collection.AsQueryable().ToListAsync();
        }
    }
}
