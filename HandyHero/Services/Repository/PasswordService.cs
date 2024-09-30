using System;
using System.Threading.Tasks;
using HandyHero.Common;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;

namespace HandyHero.Services.Repository
{
    public class PasswordService : IPasswordService
    {
        private static string _tempEmailForVerification; // Temporary storage for email
        private readonly ICustomer _customer;
        private readonly IFieldWorker _fieldworker;
        private readonly IEmailService _emailService;

        public PasswordService(ICustomer customer, IFieldWorker fieldworker, IEmailService emailService)
        {
            _customer = customer;
            _fieldworker = fieldworker;
            _emailService = emailService;
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            _tempEmailForVerification = email;
            var customer = _customer.GetByMail(email);
            var fieldworker = _fieldworker.GetFieldWorkerByEmail(email);

            if (customer != null)
            {
                // Generate verification code
                var verificationCode = Guid.NewGuid().ToString("N").Substring(0, 6);
                customer.VerificationCode = verificationCode;
                await _customer.Update(customer);
                await _emailService.SendVerificationCodeAsync(customer.Email, verificationCode);
            }
            else if (fieldworker != null)
            {
                // Generate verification code
                var verificationCode = Guid.NewGuid().ToString("N").Substring(0, 6);
                fieldworker.VerificationCode = verificationCode;
                await _fieldworker.UpdateFieldWorkerAsync(fieldworker);
                await _emailService.SendVerificationCodeAsync(fieldworker.Email, verificationCode);
            }
            else
            {
                throw new Exception("Email not found");
            }
        }


        public async Task<bool> VerifyOtpAsync(string verificationCode)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var customer = _customer.GetByMail(email);
            var fieldworker = _fieldworker.GetFieldWorkerByEmail(email);

            if (customer != null && customer.VerificationCode == verificationCode)
            {
                return true;
            }
            else if (fieldworker != null && fieldworker.VerificationCode == verificationCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPasswordAsync(string newPassword)
        {
            var email = _tempEmailForVerification; // Use the stored email
            var customer = _customer.GetByMail(email);
            var fieldworker = _fieldworker.GetFieldWorkerByEmail(email);

            if (customer != null)
            {
                PasswordHash ph = new PasswordHash();
                customer.Password = ph.HashPassword(newPassword);
                customer.VerificationCode = null; // Clear verification code after reset
                await _customer.Update(customer);
                return true;
            }
            else if (fieldworker != null)
            {
                PasswordHash ph = new PasswordHash();
                fieldworker.Password = ph.HashPassword(newPassword);
                fieldworker.VerificationCode = null; // Clear verification code after reset
                await _fieldworker.UpdateFieldWorkerAsync(fieldworker);
                return true;
            }

            return false; // If no user matched
        }
    }
}
