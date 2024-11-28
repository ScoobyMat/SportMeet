using Application.Dtos;
using Application.Dtos.EventDtos;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(int eventId);
        Task<EventDto?> AddEventAsync(EventCreateDto eventCreateDto);
        Task<bool> UpdateEventAsync(EventUpdateDto eventUpdateDto);
        Task<bool> DeleteEventAsync(int eventId);
    }
}
