using Application.Dtos.PrivateMessageDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PrivateChatHub : Hub
    {
        private readonly IPrivateMessageService _privateMessageService;

        public PrivateChatHub(IPrivateMessageService privateMessageService)
        {
            _privateMessageService = privateMessageService;
        }

        public async Task JoinChat(int userAId, int userBId)
        {
            string groupName = GetGroupName(userAId, userBId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var history = await _privateMessageService.GetConversationAsync(userAId, userBId);
            await Clients.Caller.SendAsync("ReceiveChatHistory", history);
        }

        public async Task SendMessage(int senderId, int recipientId, string context)
        {
            string groupName = GetGroupName(senderId, recipientId);

            var createDto = new PrivateMessageCreateDto
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Content = context,
                Created = DateTime.UtcNow
            };

            var sentMessage = await _privateMessageService.SendMessageAsync(createDto);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", sentMessage);
        }

        public async Task GetChatHistory(int userAId, int userBId)
        {
            var history = await _privateMessageService.GetConversationAsync(userAId, userBId);
            await Clients.Caller.SendAsync("ReceiveChatHistory", history);
        }

        private string GetGroupName(int user1, int user2)
        {
            return user1 < user2 ? $"{user1}_{user2}" : $"{user2}_{user1}";
        }
    }
}
