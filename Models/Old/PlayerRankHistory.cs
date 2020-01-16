using System;
using System.Collections.Generic;

namespace Models.Old
{
    public class PlayerRankSeasonEntry
    {
        public PlayerRankSeasonEntry(string email, string seasonName)
        {
            Email = email;
            SeasonName = seasonName;
            RankPlots = new List<PlayerRankPlot>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string SeasonName { get; set; }
        public List<PlayerRankPlot> RankPlots { get; set; }
    }

    public class PlayerRankPlot
    {
        public PlayerRankPlot(DateTime date, int rank, int eloRating)
        {
            Date = date;
            Rank = rank;
            EloRating = eloRating;
        }

        public DateTime Date { get; set; }
        public int Rank { get; set; }
        public int EloRating { get; set; }
    }
}
