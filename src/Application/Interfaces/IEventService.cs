using Application.DTOs;
using Application.DTOs.Event;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetEvents();
        Task<EventDto?> GetEvent(int eventId);
        Task<EventDto?> AddEvent(EventCreateDto eventCreateDto);
        Task<bool> UpdateEvent(EventUpdateDto eventUpdateDto);
        Task<bool> DeleteEvent(int eventId);
    }
}
