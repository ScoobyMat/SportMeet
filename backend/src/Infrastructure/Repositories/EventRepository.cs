using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;

        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetByIdAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Attendees)
                .SingleOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Attendees)
                .ToListAsync();
        }

        public async Task AddAsync(Event ev)
        {
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event ev)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetManagedEventsAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Attendees)
                .Where(e => e.CreatedByUserId == userId)
                .ToListAsync();
        }

        public async Task<List<Event>> GetUpcomingEventsAsync()
        {
            return await _context.Events
                .Where(e => e.Status == EventStatus.Coming)
                .Include(e => e.Attendees)
                .ToListAsync();
        }

        public async Task<List<Event>> GetPastEventsAsync()
        {
            return await _context.Events
                .Where(e => e.Status == EventStatus.Completed)
                .Include(e => e.Attendees)
                .ToListAsync();
        }

        public async Task<List<Event>> GetUpcomingEventsForUserAsync(int userId)
        {
            return await _context.Events
                .Where(e =>
                    (e.CreatedByUserId == userId ||
                     (e.Attendees != null && e.Attendees.Any(a => a.UserId == userId)))
                    && e.Status == EventStatus.Coming)
                .Include(e => e.Attendees)
                .ToListAsync();
        }

        public async Task<List<Event>> GetPastEventsForUserAsync(int userId)
        {
            return await _context.Events
                .Where(e =>
                    (e.CreatedByUserId == userId ||
                     (e.Attendees != null && e.Attendees.Any(a => a.UserId == userId)))
                    && e.Status == EventStatus.Completed)
                .Include(e => e.Attendees)
                .ToListAsync();
        }


        public async Task<List<Event>> SearchEventsAsync(string? city, DateOnly? from, DateOnly? to, string? sportType)
        {
            var query = _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Attendees)
                .AsQueryable();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(e => e.City.ToLower() == city.ToLower());
            }

            if (from.HasValue)
            {
                query = query.Where(e => e.Date >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(e => e.Date <= to.Value);
            }

            if (!string.IsNullOrEmpty(sportType))
            {
                query = query.Where(e => e.SportType.ToLower() == sportType.ToLower());
            }

            query = query.OrderBy(e => e.Date);

            return await query.ToListAsync();
        }
    }
}
