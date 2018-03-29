using System.Collections.Generic;

namespace Foosball.RequestResponse
{
    public class UserMappingsResponseEntry
    {
        public string Email { get; set; }
        public List<string> Roles { get;set; }
    }
}