﻿namespace Foosball.RequestResponse
{
    public class ChangeUserPasswordRequest
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
    }
}