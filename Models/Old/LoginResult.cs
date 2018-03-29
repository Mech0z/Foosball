namespace Models.Old
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public LoginToken LoginToken { get; set; }
        public bool LoginFailed { get; set; } 
    }
}