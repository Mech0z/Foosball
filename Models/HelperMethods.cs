using System;
using System.Collections.Generic;
using System.Linq;
using Models.Old;

namespace Models
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

        public static Season GetSeasonNameOfMatch(List<Season> seasons, Match match)
        {
            var season = GetSeasonOfDate(seasons, match.TimeStampUtc);
            return season;
        }

        public static Season? GetNextSeason(List<Season> seasons, Season currentSeason)
        {
            //Ensure we have the correct object instance
            var currentSeasonFromList = seasons.Single(x => x.Name == currentSeason.Name);

            var sorted = seasons.OrderBy(x => x.StartDate).ToList();
            var indexOfCurrentSeason = sorted.IndexOf(currentSeasonFromList);

            return seasons.Count - 1 <= indexOfCurrentSeason ? null : sorted[indexOfCurrentSeason + 1];
        }
    }
}