namespace Models.RequestResponses
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DeviceName { get; set; }
        public bool RememberMe { get; set; }
    }
}
