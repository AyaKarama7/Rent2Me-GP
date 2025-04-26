using Microsoft.AspNetCore.SignalR;
using Rent2Me.Models;
using System;
using System.Threading.Tasks;

namespace Rent2Me.DTO
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message, NotificationType type)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
