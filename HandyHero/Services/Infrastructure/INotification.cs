using HandyHero.Models;

namespace HandyHero.Services.Infrastructure
{
    public interface INotification
    {
        IEnumerable<Notification> GetNotificationsByUserId(int userId);
        Notification GetNotificationById(int id);
        bool CreateNotification(Notification notification);
        bool MarkAsRead(int id);
        bool DeleteNotification(int id);
    }
}
