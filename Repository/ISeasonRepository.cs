using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public interface ISeasonRepository
    {
        List<Season> GetSeasons();
        Season GetSeason(string seasonName);
        void CreateNewSeason(Season season);
        void EndSeason(Season season);
    }

    public class SeasonRepository : BaseRepository<Season>, ISeasonRepository
    {
        public SeasonRepository(IOptions<MongoDbSettings> settings) : base(settings, "Seasons")
        {

        }

        public List<Season> GetSeasons()
        {
            var seasons =
                from e in Collection.AsQueryable()
                select e;

            return IAsyncCursorSourceExtensions.ToList(seasons);
        }

        public void CreateNewSeason(Season season)
        {
            Collection.InsertOneAsync(season);
        }

        public void EndSeason(Season season)
        {
            var seasons =
                from e in Collection.AsQueryable()
                where e.Name == season.Name
                select e;


            var currentSeason = seasons.FirstOrDefault();
            currentSeason.EndDate = DateTime.UtcNow.Date.AddHours(23);
            Collection.InsertOne(currentSeason);
        }

        public Season GetSeason(string seasonName)
        {
            var result = Collection.AsQueryable().SingleOrDefault(x => x.Name == seasonName);

            return result;
        }
    }
}