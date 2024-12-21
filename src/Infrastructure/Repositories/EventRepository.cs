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

        public async Task AddAsync(Event AddEvent)
        {
            await _context.Events.AddAsync(AddEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event DeleteEvent)
        {
            _context.Events.Remove(DeleteEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .ThenInclude(gm => gm.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .ThenInclude(gm => gm.User)
                .ToListAsync();
        }

        public async Task UpdateAsync(Event UpdateEvent)
        {
            _context.Events.Update(UpdateEvent);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Event>> GetFilteredEventsAsync(string? location, string? sportType, DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                location = location.ToLower();
                query = query.Where(e => e.City.ToLower().Contains(location));
            }

            if (!string.IsNullOrEmpty(sportType))
            {
                sportType = sportType.ToLower();
                query = query.Where(e => e.SportType.ToLower().Contains(sportType));
            }

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            return await query
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .ThenInclude(gm => gm.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsForUserAsync(int userId)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var upcomingEvents = await _context.Events
                .Where(e => e.Date >= currentDate && e.Group.Members.Any(m => m.UserId == userId))
                .OrderBy(e => e.Date)
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .ThenInclude(gm => gm.User)
                .ToListAsync();

            return upcomingEvents;
        }
    }
}
