using API.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly ILogger<PresenceHub> _logger;
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker, ILogger<PresenceHub> logger)
        {
            _tracker = tracker;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User == null)
                throw new HubException("Cannot get current user claim");

            var username = Context.User.GetUsername();

            if (string.IsNullOrEmpty(username))
                throw new HubException("Username is null or empty");

            await _tracker.UserConnected(username, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", username);

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                if (Context.User == null)
                {
                    throw new HubException("Cannot get current user claim");
                }

                await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
                await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());

                var currentUsers = await _tracker.GetOnlineUsers();
                await Clients.Others.SendAsync("GetOnlineUsers", currentUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w OnDisconnectedAsync.");
                throw;
            }
        }
    }
}