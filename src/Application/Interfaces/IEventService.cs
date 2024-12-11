using Application.Dtos.EventDtos;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(int eventId);
        Task<List<EventDto>> GetFilteredEventsAsync(string? location, DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<EventDto>> GetUpcomingEventsForUserAsync(int userId);
        Task<EventDto?> AddEventAsync(EventCreateDto eventCreateDto);
        Task UpdateEventAsync(EventUpdateDto eventUpdateDto);
        Task DeleteEventAsync(int eventId);
    }
}
