using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(IOptions<MongoDbSettings> settings) : base(settings, "MatchesV3")
        {

        }

        public List<Match> GetMatches(string season)
        {
            IMongoQueryable<Match> result;
            if (season == null)
            {
                result = Collection.AsQueryable();
            }
            else
            {
                result = Collection.AsQueryable().Where(x => x.SeasonName == season);
            }

            return result.ToList();
        }

        public List<string> GetUniqueEmails()
        {
            var matches = Collection.AsQueryable();
            var uniqueEmails = new List<string>();

            foreach (Match match in matches)
            {
                foreach (string email in match.PlayerList)
                {
                    if (!uniqueEmails.Contains(email))
                    {
                        uniqueEmails.Add(email);
                    }
                }
            }

            return uniqueEmails;
        }

        public Match GetByTimeStamp(DateTime time)
        {
            var result = Collection.AsQueryable().Where(x => x.TimeStampUtc == time);

            return result.FirstOrDefault();
        }

        public IEnumerable<Match> GetMatchesByTimeStamp(DateTime time)
        {
            var result = Collection.AsQueryable().Where(x => x.TimeStampUtc >= time);
            
            return result;
        }

        public List<Match> GetRecentMatches(int numberOfMatches)
        {
            var result = Collection.AsQueryable().OrderByDescending(x => x.TimeStampUtc);
            
            return result.Take(numberOfMatches).ToList();
        }

        public List<Match> GetPlayerMatches(string email)
        {
            var result = Collection.AsQueryable().Where(x => x.PlayerList.Contains(email));
            
            return result.ToList();
        }

        public void Upsert(Match match)
        {
            Collection.ReplaceOne(i => i.Id == match.Id, match,
                            new UpdateOptions { IsUpsert = true });
        }
    }
}