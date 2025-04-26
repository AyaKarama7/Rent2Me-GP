using Rent2Me.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR;

namespace Rent2Me.DTO
{
    public class NotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(string userId, string message, NotificationType type)
        {
            var notification = new Notification
            {
                CustomerId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = type
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Notify the user in real-time via SignalR
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }
        public async Task SendNotification(Notification notification)
        {
            await _hubContext.Clients.User(notification.CustomerId).SendAsync("ReceiveNotification", notification);
        }
    }


}
