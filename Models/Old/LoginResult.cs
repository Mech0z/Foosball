using System.Collections.Generic;

namespace Models.Old
{
    public class LoginResult
    {
        public LoginResult()
        {
            Roles = new List<string>();
        }

        public bool Success { get; set; }
        public LoginToken LoginToken { get; set; }
        public bool LoginFailed { get; set; } 
        public List<string> Roles { get; set; }
        public bool Expired { get; set; }
    }
}