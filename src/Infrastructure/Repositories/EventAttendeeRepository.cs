using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventAttendeeRepository : IEventAttendeeRepository
    {
        private readonly DataContext _context;

        public EventAttendeeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EventAttendee?> GetByIdAsync(int id)
        {
            return await _context.EventAttendees.FindAsync(id);
        }

        public async Task<IEnumerable<EventAttendee>> GetByEventIdAsync(int eventId)
        {
            return await _context.EventAttendees
                .Where(a => a.EventId == eventId)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<EventAttendee?> GetByEventAndUserAsync(int eventId, int userId)
        {
            return await _context.EventAttendees
                .FirstOrDefaultAsync(a => a.EventId == eventId && a.UserId == userId);
        }

        public async Task AddAsync(EventAttendee attendee)
        {
            await _context.EventAttendees.AddAsync(attendee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EventAttendee attendee)
        {
            _context.EventAttendees.Remove(attendee);
            await _context.SaveChangesAsync();
        }
    }
}
