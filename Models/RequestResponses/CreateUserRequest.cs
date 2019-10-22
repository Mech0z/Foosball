namespace Models.RequestResponses
{
    public class CreateUserRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
    }
}