using Application.Dtos.EventMessageDtos;

namespace Application.Interfaces
{
    public interface IEventMessageService
    {
        Task<IEnumerable<EventMessageDto>> GetMessagesForEventAsync(int eventId);
        Task<EventMessageDto> SendMessageAsync(EventMessageCreateDto dto);
        Task DeleteMessageAsync(int messageId);
    }
}
