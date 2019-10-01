using System.Collections.Generic;
using System.Threading.Tasks;
using Foosball.RequestResponse;
using Models.Old;

namespace Foosball.Logic
{
    public interface ISeasonLogic
    {
        Task<string> StartNewSeason(UpsertSeasonRequest request);
        Task<List<Season>> GetSeasons();
        Task<List<Season>> GetStartedSeasons();
        Task<Season> GetActiveSeason();
    }
}