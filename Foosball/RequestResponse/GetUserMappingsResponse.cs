using System.Collections.Generic;
using Models;

namespace Foosball.RequestResponse
{
    public class GetUserMappingsResponse
    {
        public List<UserMapping> UserMappings { get; set; }
        public List<string> IdentityEmails { get; set; }
        public List<string> NormalUsernames { get; set; }
    }
}