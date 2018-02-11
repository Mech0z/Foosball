using System;
using System.Collections.Generic;
using Models.Old;

namespace Repository
{
    public interface IMatchRepository
    {
        void Upsert(Match match);
        List<Match> GetMatches(string season);
        Match GetByTimeStamp(DateTime dateTime);
        List<Match> GetRecentMatches(int numberOfMatches);
        List<Match> GetPlayerMatches(string email);
        IEnumerable<Match> GetMatchesByTimeStamp(DateTime time);
        List<string> GetUniqueEmails();
    }
}