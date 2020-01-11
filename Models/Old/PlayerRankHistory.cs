using System;
using System.Collections.Generic;

namespace Models.Old
{
    public class PlayerRankHistory
    {
        public PlayerRankHistory(string email)
        {
            Email = email;
            PlayerRankHistorySeasonEntries = new List<PlayerRankHistorySeasonEntry>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<PlayerRankHistorySeasonEntry> PlayerRankHistorySeasonEntries { get; set; }
    }

    public class PlayerRankHistorySeasonEntry
    {
        public PlayerRankHistorySeasonEntry(string seasonName)
        {
            SeasonName = seasonName;
            HistoryPlots = new List<PlayerRankHistoryPlot>();
        }

        public string SeasonName { get; set; }
        public List<PlayerRankHistoryPlot> HistoryPlots { get; set; }
    }

    public class PlayerRankHistoryPlot
    {
        public PlayerRankHistoryPlot(DateTime date, int rank, int eloRating)
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
