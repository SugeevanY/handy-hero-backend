using HandyHero.Data;
using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;

namespace HandyHero.Services.Repository
{
    public class ComplaintRepository : IComplaint
    {
        private ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool createComplaint(Complaint complaint)
        {
            try
            {
                _context.Complaint.Add(complaint);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public List<ComplaintView> GetComplaints()
        {
            var complaints = _context.Complaint.ToList();
            var complaintViewModels = new List<ComplaintView>();

            foreach (var complaint in complaints)
            {
                var accusedEmail = complaint.Accused;
                var accused = _context.FieldWorker.FirstOrDefault(f => f.Email == accusedEmail);

                if (accused != null)
                {
                    var complaintViewModel = new ComplaintView
                    {
                        AccusedEmail = accused.Email,
                        Status = accused.Status,
                        ComplaintMessage = complaint.ComplaintMessage
                    };

                    complaintViewModels.Add(complaintViewModel);
                }
            }

            return complaintViewModels;
        }
    }
}

