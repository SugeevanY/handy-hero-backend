using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HandyHero.Common;
using HandyHero.Services.Infrastructure;
using Microsoft.Extensions.Options;

namespace HandyHero.Services.Repository
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationCode))
            {
                throw new ArgumentNullException(nameof(verificationCode), "Verification code cannot be null or empty.");
            }

            // Validate email
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            // Retrieve email configuration values
            var smtpHost = _config["EmailConfiguration:Host"];
            var smtpPort = _config["EmailConfiguration:Port"];
            var smtpUsername = _config["EmailConfiguration:Username"];
            var smtpPassword = _config["EmailConfiguration:Password"];
            var smtpFrom = _config["EmailConfiguration:From"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPort) ||
                string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword) ||
                string.IsNullOrEmpty(smtpFrom))
            {
                throw new InvalidOperationException("Email configuration settings are not properly set.");
            }

            // Create and configure the SMTP client
            using (var client = new SmtpClient(smtpHost, int.Parse(smtpPort)))
            {
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                // Create the mail message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpFrom),
                    Subject = "Password Reset Verification Code",
                    Body = $"Your verification code is: {verificationCode}",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                try
                {
                    // Send the email
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logging framework or log to file/db as needed)
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw new InvalidOperationException("Failed to send verification email.", ex);
                }
            }
        }

    }
}


