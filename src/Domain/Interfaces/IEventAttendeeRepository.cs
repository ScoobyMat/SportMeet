using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventAttendeeRepository
    {
        Task<EventAttendee?> GetByIdAsync(int id);
        Task<IEnumerable<EventAttendee>> GetByEventIdAsync(int eventId);
        Task AddAsync(EventAttendee attendee);
        Task UpdateAsync(EventAttendee attendee);
        Task DeleteAsync(EventAttendee attendee);
    }
}
