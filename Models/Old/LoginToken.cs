using System;

namespace Models.Old
{
    public class LoginToken
    {
        public string Token { get; set; }
        public DateTime Expirytime { get; set; }
        public string DeviceName { get; set; }
    }
}