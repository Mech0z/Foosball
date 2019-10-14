using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Old
{
    [BsonIgnoreExtraElements]
    public class Match
    {
        public Match()
        {
            PlayerList = new List<string>();
        }

        public Match(DateTime timeStampUtc,
            List<string> playerList,
            MatchResult matchResult,
            int points,
            string submittedBy)
        {
            TimeStampUtc = timeStampUtc;
            PlayerList = playerList;
            MatchResult = matchResult;
            Points = points;
            SubmittedBy = submittedBy;
        }

        public Guid Id { get; set; }

        public DateTime TimeStampUtc { get; set; }

        /// <summary>
        /// Ordered:
        /// Team 1
        /// Team 1
        /// Team 2
        /// Team 2
        /// </summary>
        public List<string> PlayerList { get; set; }
        
        [NotMapped]
        public int Team1HashCode => PlayerList.Take(2).OrderBy(x => x).GetHashCode();
        [NotMapped]
        public int Team2HashCode => PlayerList.TakeLast(2).OrderBy(x => x).GetHashCode();

        public MatchResult? MatchResult { get; set; }

        public int Points { get; set; }

        public string? SubmittedBy { get; set; }

        public EditedType? EditedType { get; set; }
    }
}