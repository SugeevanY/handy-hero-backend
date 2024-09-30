namespace HandyHero.Services.Infrastructure
{
    public interface IMailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
