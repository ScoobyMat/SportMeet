using Application.Dtos.EventDtos;

namespace Application.Interfaces
{
    public interface IEventService
    {
        Task<EventDto> CreateAsync(EventCreateDto dto);
        Task<EventDto> UpdateAsync(EventUpdateDto dto);
        Task<EventDto?> GetByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task DeleteAsync(int id);

        Task JoinEventAsync(int eventId, int userId);
        // ewentualnie: Task LeaveEventAsync(int eventId, int userId);

        Task<List<EventDto>> GetUpcomingEventsForUserAsync(int userId);
        Task<List<EventDto>> GetManagedEventsAsync(int userId);
        Task<List<EventDto>> SearchEventsAsync(EventSearchDto searchDto);
    }
}
