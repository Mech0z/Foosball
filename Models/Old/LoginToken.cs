using System;

namespace Models.Old
{
    public class LoginToken
    {
        public LoginToken(string token, DateTime expirytime, string deviceName)
        {
            Token = token;
            Expirytime = expirytime;
            DeviceName = deviceName;
        }

        public string Token { get; set; }
        public DateTime Expirytime { get; set; }
        public string DeviceName { get; set; }
    }
}