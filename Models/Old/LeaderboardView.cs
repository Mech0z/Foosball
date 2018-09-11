using System;
using System.Collections.Generic;

namespace Models.Old
{
    public class LeaderboardView
    {
        public LeaderboardView()
        {
            Entries = new List<LeaderboardViewEntry>();
            Timestamp = DateTime.UtcNow;
        }
        
        public Guid Id { get; set; }
        public List<LeaderboardViewEntry> Entries { get; set; }
        public string SeasonName { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}