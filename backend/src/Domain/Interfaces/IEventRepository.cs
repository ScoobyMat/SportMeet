using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(int eventId);
        Task<IEnumerable<Event>> GetAllAsync();
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(Event ev);

        Task<List<Event>> GetManagedEventsAsync(int userId);
        Task<List<Event>> GetUpcomingEventsAsync();
        Task<List<Event>> GetPastEventsAsync();
        Task<List<Event>> GetUpcomingEventsForUserAsync(int userId);
        Task<List<Event>> GetPastEventsForUserAsync(int userId);

        Task<List<Event>> SearchEventsAsync(string? city, DateOnly? from, DateOnly? to, string? sportType);

    }
}
