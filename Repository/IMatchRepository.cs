using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.Old;

namespace Repository
{
    public interface IMatchRepository
    {
        Task Upsert(Match match);
        Task<long> UnDeleteMatch(Guid matchId);
        Task<List<Match>> GetDeletedMatches();
        Task<List<Match>> GetMatches(DateTime? startDate, DateTime? dateOfNextSeasonStart);
        Task<Match> GetByTimeStamp(DateTime dateTime);
        Task<List<Match>> GetRecentMatches(int numberOfMatches);
        Task<List<Match>> GetPlayerMatches(string email);
        Task<List<Match>> GetMatchesByTimeStamp(DateTime time);
        Task<List<string>> GetUniqueEmails();
        Task<long> DeleteMatch(Guid matchId, LoginSession loginSession);
    }
}