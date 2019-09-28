using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public static class HelperMethods
    {
        public static Season GetCurrentSeason(List<Season> seasons)
        {
            var currentSeason = GetSeasonOfDate(seasons, DateTime.Today);
            return currentSeason;
        }

        public static Season GetSeasonOfDate(List<Season> seasons, DateTime date)
        {
            var currentSeason = seasons
                .Where(x => x.StartDate <= date)
                .OrderBy(x => x.StartDate)
                .LastOrDefault();
            return currentSeason;
        }
    }
}
