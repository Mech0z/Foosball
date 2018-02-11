using System.Collections.Generic;
using Models.Old;

namespace FoosballCore.OldLogic
{
    public interface IMatchupHistoryCreator : ILogic
    {
        List<PartnerPercentResult> GetPartnerWinPercent(string email, string season);
        void AddMatch(Match match);
        void RecalculateMatchupHistory();
    }
}