using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "MatchesV3")
        {

        }

        public async Task<List<Match>> GetMatches(string season)
        {
            var result = string.IsNullOrWhiteSpace(season)
                ? Collection.AsQueryable()
                : Collection.AsQueryable()
                    .Where(x => x.SeasonName == season && (x.EditedType == null || x.EditedType.Deleted == false));

            return await result.ToListAsync();
        }

        public async Task<List<string>> GetUniqueEmails()
        {
            var matches = await Collection.AsQueryable().ToListAsync();
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

        public async Task<Match> GetByTimeStamp(DateTime time)
        {
            var result = Collection.AsQueryable().Where(x => x.TimeStampUtc == time);

            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<Match>> GetMatchesByTimeStamp(DateTime time)
        {
            var result = Collection.AsQueryable().Where(x => x.TimeStampUtc >= time);
            
            return await result.ToListAsync();
        }

        public async Task<List<Match>> GetRecentMatches(int numberOfMatches)
        {
            var result = Collection.AsQueryable()
                .Where(x => (x.EditedType == null || x.EditedType.Deleted == false))
                .OrderByDescending(x => x.TimeStampUtc)
                .Take(numberOfMatches);
            
            return await result.ToListAsync();
        }

        public async Task<List<Match>> GetPlayerMatches(string email)
        {
            var result = Collection.AsQueryable()
                .Where(x => x.PlayerList.Contains(email) && (x.EditedType == null || x.EditedType.Deleted == false));
            
            return await result.ToListAsync();
        }

        public async Task Upsert(Match match)
        {
            if (match.Id == Guid.Empty)
            {
                await Collection.InsertOneAsync(match);
            }
            else
            {
                await Collection.ReplaceOneAsync(i => i.Id == match.Id, match);
            }
        }

        public async Task<long> DeleteMatch(Guid matchId, LoginSession loginSession)
        {
            Match match = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Id == matchId);
            if (match != null)
            {
                match.EditedType = new EditedType
                {
                    DeleteTime = DateTime.Now,
                    Deleted = true,
                    DeletedBy = loginSession.Email
                };

                await Collection.ReplaceOneAsync(i => i.Id == match.Id, match);
                return 1;
            }
            
            return 0;
            //var deleteResult = await Collection.DeleteOneAsync(x => x.Id == matchId);
            //return deleteResult.DeletedCount;
        }
    }
}