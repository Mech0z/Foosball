using System;

namespace Foosball.RequestResponse
{
    public class LoginResponse
    {
        public DateTime ExpiryTime { get; set; }
        public string Token { get; set; }
    }
}