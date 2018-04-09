using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IMatchupResultRepository
    {
        Task Upsert(MatchupResult matchupResult);
        List<MatchupResult> GetByHashResult(int hashcode);
    }
}