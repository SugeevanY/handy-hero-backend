using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using HandyHero.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandyHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _notification;

        public NotificationController(INotification notificationRepository)
        {
            _notification = notificationRepository;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Notification>> GetNotificationsByUserId(int userId)
        {
            var notifications = _notification.GetNotificationsByUserId(userId);
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public ActionResult<Notification> GetNotificationById(int id)
        {
            var notification = _notification.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpPost]
        public ActionResult CreateNotification([FromBody] Notification notification)
        {
            var result = _notification.CreateNotification(notification);
            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the notification.");
            }
            return Ok();
        }

        [HttpPut("markAsRead/{id}")]
        public ActionResult MarkAsRead(int id)
        {
            var result = _notification.MarkAsRead(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteNotification(int id)
        {
            var result = _notification.DeleteNotification(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
    

