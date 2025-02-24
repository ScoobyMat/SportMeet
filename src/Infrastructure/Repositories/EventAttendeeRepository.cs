using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await _context.EventAttendees
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<EventAttendee>> GetByEventIdAsync(int eventId)
        {
            return await _context.EventAttendees
                .Include(a => a.User) 
                .Where(a => a.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(EventAttendee attendee)
        {
            await _context.EventAttendees.AddAsync(attendee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventAttendee attendee)
        {
            _context.EventAttendees.Update(attendee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EventAttendee attendee)
        {
            _context.EventAttendees.Remove(attendee);
            await _context.SaveChangesAsync();
        }
    }
}
