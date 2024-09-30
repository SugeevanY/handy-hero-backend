using HandyHero.Models;

namespace HandyHero.Services.Infrastructure
{
    public interface ICustomer
    {
        public bool Login(string email, string password);
        public bool SignUp(Customer customer);
        public bool SignOut();
        public bool ResetPassword(Customer customer);
        public IEnumerable<Object> getMyProject(int Id);
        public IEnumerable<FieldWorker> findWorker(string WorkType);
        public bool createProject(Project project);

        public Task<bool> Update(Customer customer);

        public Customer GetByMail(string Email);

        public Customer getCustomerByMail(string Email);
        public bool createComplaint(Complaint complaint);

        public Customer findCustomerById(int Id);


    }
}
