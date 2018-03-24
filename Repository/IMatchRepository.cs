using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IMatchRepository
    {
        Task Upsert(Match match);
        Task<List<Match>> GetMatches(string season);
        Task<Match> GetByTimeStamp(DateTime dateTime);
        Task<List<Match>> GetRecentMatches(int numberOfMatches);
        Task<List<Match>> GetPlayerMatches(string email);
        Task<List<Match>> GetMatchesByTimeStamp(DateTime time);
        Task<List<string>> GetUniqueEmails();
    }
}