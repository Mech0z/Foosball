using System.Collections.Generic;

namespace Models.RequestResponses
{
    public class UserMappingsResponseEntry
    {
        public UserMappingsResponseEntry(string email, List<string> roles)
        {
            Email = email;
            Roles = roles;
        }

        public string Email { get; set; }
        public List<string> Roles { get;set; }
    }
}