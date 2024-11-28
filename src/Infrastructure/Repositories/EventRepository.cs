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
    }
}
