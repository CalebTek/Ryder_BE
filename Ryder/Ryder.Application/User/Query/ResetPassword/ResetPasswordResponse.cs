﻿namespace Ryder.Application.User.Query.ResetPassword
{
    public class ResetPasswordResponse
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ResetToken { get; set; }
    }
}
