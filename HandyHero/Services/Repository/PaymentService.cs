using HandyHero.Data;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandyHero.Services.Repository
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            try
            {
                return await _context.Payments.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                throw new Exception("An error occurred while retrieving payments.", ex);
            }
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                throw new Exception("An error occurred while adding the payment.", ex);
            }
        }
    }
}
