using HandyHero.Services.Infrastructure;
using System.Net.Mail;

namespace HandyHero.Services.Repository
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        public MailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage("viyukshanak24@gmail.com", to, subject, body);
            _smtpClient.Send(mailMessage);
        }
    }
}
