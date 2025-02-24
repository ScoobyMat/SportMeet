using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEventMessageRepository
    {
        Task<EventMessage?> GetByIdAsync(int id);
        Task<IEnumerable<EventMessage>> GetAllByEventIdAsync(int eventId);
        Task AddAsync(EventMessage message);
        Task UpdateAsync(EventMessage message);
        Task DeleteAsync(EventMessage message);
    }
}
