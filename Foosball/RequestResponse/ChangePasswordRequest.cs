﻿namespace Foosball.RequestResponse
{
    public class ChangePasswordRequest
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
    }
}