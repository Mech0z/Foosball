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
    public interface ISeasonRepository
    {
        Task<List<Season>> GetSeasons();
        Task<Season> GetSeason(string seasonName);
        Task<List<Season>> GetStartedSeasons();
        Task CreateNewSeason(Season season);
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

        public async Task<List<Season>> GetStartedSeasons()
        {
            var seasons =
                from e in Collection.AsQueryable()
                    .Where(x => x.StartDate <= DateTime.Today)
                select e;
            return await seasons.ToListAsync();
        }

        public async Task CreateNewSeason(Season season)
        {
            await Collection.InsertOneAsync(season);
        }

        //public async Task EndSeason(Season season)
        //{
        //    var seasons =
        //        from e in Collection.AsQueryable()
        //        where e.Name == season.Name
        //        select e;


        //    var currentSeason = seasons.FirstOrDefault();
        //    currentSeason.EndDate = DateTime.UtcNow.Date.AddHours(23);
        //    await Collection.ReplaceOneAsync(x => x.Name == currentSeason.Name, currentSeason);
        //}

        public async Task<Season> GetSeason(string seasonName)
        {
            var result = await Collection
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Name == seasonName);

            return result;
        }
    }
}