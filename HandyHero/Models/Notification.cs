using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; } // Primary key


        public string Title { get; set; } // Title of the notification

        public string Message { get; set; } // The main content/message of the notification

        public DateTime CreatedAt { get; set; } // Timestamp of when the notification was created

        //public bool IsRead { get; set; } // Flag to check if the notification has been read

        public int UserId { get; set; } // ID of the user to whom the notification is addressed
        public bool IsRead { get; internal set; }

        // Navigation property to the User
         //public virtual User User { get; set; }
    }
}
