using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IActivityRepository
    {
        Task UpsertActivity(Activity activity);
        Task<Activity> GetActivityByDate(DateTime date);
        Task<List<Activity>> GetActivities();
    }
}