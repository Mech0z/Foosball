using System.Collections.Generic;
using Models.Old;

namespace FoosballCore.OldLogic
{
    public interface ISeasonLogic
    {
        string StartNewSeason();
        List<Season> GetSeasons();
        Season GetActiveSeason();
    }
}