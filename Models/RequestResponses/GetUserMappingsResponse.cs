using System.Collections.Generic;

namespace Models.RequestResponses
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