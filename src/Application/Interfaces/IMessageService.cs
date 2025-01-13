using Application.Dtos.MessageDtos;

namespace Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> GetMessageByIdAsync(int messageId);
        Task<List<MessageDto>> GetMessagesForGroupAsync(int groupId);
        Task<MessageDto> AddMessageAsync(CreateMessageDto createMessageDto);
    }
}
