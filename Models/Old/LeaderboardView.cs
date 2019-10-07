﻿using System;
using System.Collections.Generic;

namespace Models.Old
{
    public class LeaderboardView
    {
        public LeaderboardView(string seasonName)
        {
            Entries = new List<LeaderboardViewEntry>();
            Timestamp = DateTime.UtcNow;
            SeasonName = seasonName;
        }
        
        public Guid Id { get; set; }
        public List<LeaderboardViewEntry> Entries { get; set; }
        public string SeasonName { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime StartDate { get; set; }
    }
}