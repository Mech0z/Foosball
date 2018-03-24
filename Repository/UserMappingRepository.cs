using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

namespace Repository
{
    public class UserMappingRepository: BaseRepository<UserMapping>, IUserMappingRepository
    {
        public UserMappingRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "Usermappings")
        {
            
        }
        
        public async Task<List<UserMapping>> GetUserMappings()
        {
            var result = Collection.AsQueryable();
            return await result.ToListAsync();
        }

        public async Task SaveUserMapping(UserMapping userMapping)
        {
            await Collection.ReplaceOneAsync(i => i.Id == userMapping.Id, userMapping,
                new UpdateOptions { IsUpsert = true });
        }
    }
}