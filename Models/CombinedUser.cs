using System.Collections.Generic;

namespace Models
{
    public class CombinedUser
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}