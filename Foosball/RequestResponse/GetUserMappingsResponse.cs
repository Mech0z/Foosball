using System.Collections.Generic;

namespace Foosball.RequestResponse
{
    public class GetUserMappingsResponse
    {
        public GetUserMappingsResponse()
        {
            Users = new List<UserMappingsResponseEntry>();
        }

        public List<UserMappingsResponseEntry> Users { get; set; }
    }
}