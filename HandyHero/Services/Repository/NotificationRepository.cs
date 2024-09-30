using HandyHero.Data;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;

namespace HandyHero.Services.Repository
{
    public class NotificationRepository : INotification
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetNotificationsByUserId(int userId)
        {
            return _context.Notifications.Where(n => n.UserId == userId).ToList();
        }

        public Notification GetNotificationById(int id)
        {
            return _context.Notifications.FirstOrDefault(n => n.Id == id);
        }

        public bool CreateNotification(Notification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool MarkAsRead(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification == null) return false;

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteNotification(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            _context.SaveChanges();
            return true;
        }
    }
}
    
