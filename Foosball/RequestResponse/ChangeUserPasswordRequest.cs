namespace Foosball.RequestResponse
{
    public class ChangeUserPasswordRequest : BaseRequest
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
    }
}