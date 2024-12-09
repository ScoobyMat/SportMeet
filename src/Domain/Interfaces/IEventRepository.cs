using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetFilteredEventsAsync(string? location, DateOnly? startDate, DateOnly? endDate);
        Task<IEnumerable<Event>> GetUpcomingEventsForUserAsync(int userId);
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event AddEvent);
        Task UpdateAsync(Event UpdateEvent);
        Task DeleteAsync(Event DeleteEvent);
    }
}
