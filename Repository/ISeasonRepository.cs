using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public interface ISeasonRepository
    {
        Task<List<Season>> GetSeasons();
        Season GetSeason(string seasonName);
        Task CreateNewSeason(Season season);
        Task EndSeason(Season season);
    }

    public class SeasonRepository : BaseRepository<Season>, ISeasonRepository
    {
        public SeasonRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "Seasons")
        {

        }

        public async Task<List<Season>> GetSeasons()
        {
            var seasons =
                from e in Collection.AsQueryable()
                select e;

            return await seasons.ToListAsync();
        }

        public async Task CreateNewSeason(Season season)
        {
            await Collection.InsertOneAsync(season);
        }

        public async Task EndSeason(Season season)
        {
            var seasons =
                from e in Collection.AsQueryable()
                where e.Name == season.Name
                select e;


            var currentSeason = seasons.FirstOrDefault();
            currentSeason.EndDate = DateTime.UtcNow.Date.AddHours(23);
            await Collection.ReplaceOneAsync(x => x.Name == currentSeason.Name, currentSeason);
        }

        public Season GetSeason(string seasonName)
        {
            var result = Collection.AsQueryable().SingleOrDefault(x => x.Name == seasonName);

            return result;
        }
    }
}