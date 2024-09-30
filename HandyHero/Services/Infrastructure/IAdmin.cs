using HandyHero.DTO;
using HandyHero.Models;
using System.Security.Claims;

namespace HandyHero.Services.Infrastructure
{
    public interface IAdmin
    {
        bool Login(string email, string password);
        bool CreateAdmin(Admin admin);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<FieldWorker> GetFieldWorkers();

        public bool BlockFieldWorker(FieldWorker fieldWorker);
        public bool IsAdmin(Admin admin);

        public ClaimsPrincipal validateToken(string token);

        public Admin GetAdminByEmail(string Email);
        public Admin GetAdminById(int Id);

        public bool AcceptFieldWorker(string fieldworkerEmail, int adminId);
        public bool RejectFieldWorker(string fieldWorkerId, int adminId);

        public List<ComplaintView> gettAllComplaints();

        public bool Logout();
    }
}
