using System.Collections.Generic;
using Models.Old;

namespace Repository
{
    public interface IMatchupResultRepository
    {
        void Upsert(MatchupResult matchupResult);
        List<MatchupResult> GetByHashResult(int hashcode);
    }
}