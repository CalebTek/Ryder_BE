namespace Ryder.Application.Authentication.Command.ResetPassword
{
    public class ResetPasswordResponse
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }
}