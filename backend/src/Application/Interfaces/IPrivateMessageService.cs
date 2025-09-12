using Application.Dtos.PrivateMessageDtos;

namespace Application.Interfaces
{
    public interface IPrivateMessageService
    {
        Task<IEnumerable<PrivateMessageDto>> GetConversationAsync(int userAId, int userBId);
        Task<PrivateMessageDto> SendMessageAsync(PrivateMessageCreateDto dto);
        Task DeleteMessageAsync(int messageId);
    }
}
