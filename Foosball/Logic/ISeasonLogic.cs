using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;
using Models.RequestResponses;

namespace Foosball.Logic
{
    public interface ISeasonLogic
    {
        Task<string> StartNewSeason(UpsertSeasonRequest request);
        Task<List<Season>> GetSeasons();
        Task<List<Season>> GetStartedSeasons();
        Task<Season> GetActiveSeason(List<Season>? seasons = null);
    }
}