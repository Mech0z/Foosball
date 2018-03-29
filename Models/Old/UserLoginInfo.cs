using System;
using System.Collections.Generic;

namespace Models.Old
{
    public class UserLoginInfo
    {
        public UserLoginInfo()
        {
            Tokens = new List<LoginToken>();
            Roles = new List<string>();
        }
        
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string HashedPassword { get; set; }
        public List<LoginToken> Tokens { get; set; }
    }
}