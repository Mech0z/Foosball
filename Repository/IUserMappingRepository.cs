using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public interface IUserMappingRepository
    {
        Task<List<UserMapping>> GetUserMappings();
        Task SaveUserMapping(UserMapping userMapping);
    }
}