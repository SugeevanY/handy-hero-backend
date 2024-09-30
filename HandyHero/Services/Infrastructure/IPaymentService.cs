using HandyHero.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HandyHero.Services.Infrastructure
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task AddPaymentAsync(Payment payment);
    }
}
