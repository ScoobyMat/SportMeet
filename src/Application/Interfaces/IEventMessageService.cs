using Application.Dtos.EventMessageDtos;

namespace Application.Interfaces
{
    public interface IEventMessageService
    {
        Task<IEnumerable<EventMessageDto>> GetMessagesForEventAsync(int eventId);
        Task<EventMessageDto> GetMessageByIdAsync(int id);
        Task<EventMessageDto> CreateMessageAsync(EventMessageCreateDto dto);
        Task DeleteMessageAsync(int id);
    }
}
