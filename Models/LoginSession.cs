namespace Models
{
    public class LoginSession
    {
        public LoginSession(string token, string email, string deviceName)
        {
            Token = token;
            Email = email;
            DeviceName = deviceName;
        }

        public string Token { get; set; }
        public string Email { get; set; }
        public string DeviceName { get; set; }
    }
}