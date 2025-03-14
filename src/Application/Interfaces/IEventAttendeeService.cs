using Application.Dtos.EventAttendeeDtos;

namespace Application.Interfaces
{
    public interface IEventAttendeeService
    {
        Task<EventAttendeeDto> AddAttendeeAsync(int eventId, int userId);
        Task<EventAttendeeDto?> GetByEventAndUserAsync(int eventId, int userId);
        Task<IEnumerable<EventAttendeeDto>> GetByEventIdAsync(int eventId);
        Task DeleteAttendeeAsync(int attendeeId);
    }
}
