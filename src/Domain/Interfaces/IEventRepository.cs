using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetFilteredEventsAsync(string? location, string? sportType, DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Event>> GetUpcomingEventsForUserAsync(int userId);
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event AddEvent);
        Task UpdateAsync(Event UpdateEvent);
        Task DeleteAsync(Event DeleteEvent);
    }
}
