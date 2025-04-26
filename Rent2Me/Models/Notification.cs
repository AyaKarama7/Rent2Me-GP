using System.ComponentModel.DataAnnotations;
using System;

namespace Rent2Me.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
    public enum NotificationType
    {
        SystemUpdate,
        RentalRequest,
        RentalAccepted,
        RentalRejected,
        Feedback,
    }
}
