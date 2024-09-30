namespace HandyHero.Services.Infrastructure
{
    public interface IEmailService
    {
        Task SendVerificationCodeAsync(string email, string verificationCode);

    }
}
