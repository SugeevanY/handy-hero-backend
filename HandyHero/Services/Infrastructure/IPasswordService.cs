namespace HandyHero.Services.Infrastructure
{
    public interface IPasswordService
    {
        Task SendVerificationCodeAsync(string email);
      
        public Task<bool> VerifyOtpAsync(string verificationCode);
        public Task<bool> ResetPasswordAsync(string newPassword);
    }
}
