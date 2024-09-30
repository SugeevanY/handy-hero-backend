using HandyHero.DTO;
using HandyHero.Models;

namespace HandyHero.Services.Infrastructure
{
    public interface IComplaint
    {
        public List<ComplaintView> GetComplaints();
        public bool createComplaint(Complaint complaint);
    }
}
