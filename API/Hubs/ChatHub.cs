using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }

        public async Task SaveUserConnection(int userId)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection
            {
                ConnectionId = connectionId,
                userId = userId
            };

            _context.HubConnections.Add(hubConnection);
            await _context.SaveChangesAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = await _context.HubConnections.FirstOrDefaultAsync(con => con.ConnectionId == Context.ConnectionId);

            if (hubConnection != null)
            {
                _context.HubConnections.Remove(hubConnection);
                await _context.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
