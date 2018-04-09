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
    public class MatchupResultRepository : BaseRepository<MatchupResult>, IMatchupResultRepository
    {
        public MatchupResultRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "MatchupResults")
        {

        }

        public List<MatchupResult> GetByHashResult(int hashcode)
        {
            var query = Collection.AsQueryable();

            query.Where(x => x.HashResult == hashcode);

            return query.ToList();
        }

        public async Task Upsert(MatchupResult matchupResult)
        {
            if (matchupResult.Id == Guid.Empty)
            {
                await Collection.InsertOneAsync(matchupResult);
            }
            else
            {
                await Collection.ReplaceOneAsync(i => i.Id == matchupResult.Id, matchupResult);
            }
        }
    }
}