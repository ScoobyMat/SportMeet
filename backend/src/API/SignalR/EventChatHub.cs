using Application.Interfaces;
using Application.Dtos.EventMessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class EventChatHub : Hub
    {
        private readonly IEventMessageService _eventMessageService;

        public EventChatHub(IEventMessageService eventMessageService)
        {
            _eventMessageService = eventMessageService;
        }

        public async Task JoinEventChat(int eventId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"event-{eventId}");
            var history = await _eventMessageService.GetMessagesForEventAsync(eventId);
            await Clients.Caller.SendAsync("ReceiveHistory", history);
        }

        public async Task LeaveEventChat(int eventId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"event-{eventId}");
            Console.WriteLine($"Użytkownik {Context.UserIdentifier} opuścił event-{eventId}");
        }

        public async Task SendMessage(int eventId, string user, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new HubException("Message cannot be empty.");

            if (string.IsNullOrEmpty(Context.UserIdentifier))
                throw new HubException("User is not authenticated.");

            var dto = new EventMessageCreateDto
            {
                EventId = eventId,
                SenderId = int.Parse(Context.UserIdentifier),
                Content = message
            };

            var sentMessage = await _eventMessageService.SendMessageAsync(dto);
            await Clients.Group($"event-{eventId}").SendAsync("ReceiveMessage", sentMessage);
        }

        public async Task<IEnumerable<EventMessageDto>> LoadChatHistory(int eventId)
        {
            return await _eventMessageService.GetMessagesForEventAsync(eventId);
        }
    }
}
