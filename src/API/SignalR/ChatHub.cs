using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Application.Dtos.MessageDtos;
using Application.Interfaces;
using Domain.Interfaces;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IUserRepository _userRepository;

    public ChatHub(IMessageService messageService, IUserRepository userRepository)
    {
        _messageService = messageService;
        _userRepository = userRepository;
    }

    public async Task JoinGroup(int groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());

        var history = await _messageService.GetMessagesForGroupAsync(groupId);

        await Clients.Caller.SendAsync("ReceiveHistory", history);
    }


    public async Task SendMessage(int groupId, CreateMessageDto messageDto)
    {
            var message = await _messageService.AddMessageAsync(messageDto);

            var sender = await _userRepository.GetByIdAsync(messageDto.SenderId);
            if (sender != null)
            {
                message.SenderName = $"{sender.FirstName} {sender.LastName}";
            }
            await Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage", message);
    }


    public async Task LeaveGroup(string groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
    }
}
