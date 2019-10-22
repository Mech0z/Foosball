using System;
using System.Collections.Generic;

namespace Models.RequestResponses
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            Roles = new List<string>();
        }

        public DateTime ExpiryTime { get; set; }
        public string? Token { get; set; }
        public bool LoginFailed { get; set; }
        public List<string> Roles { get; set; }
    }
}