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

        public async Task Add(Event AddEvent)
        {
           await _context.Events.AddAsync(AddEvent);
            _context.SaveChanges();
        }

        public void DeleteEvent(Event DeleteEvent)
        {
            _context.Events.Remove(DeleteEvent);
            _context.SaveChanges();
        }

        public async Task<Event?> GetEvent(int id)
        {
            return await _context.Events
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            return await _context.Events
                .Include(e => e.Group)
                .ThenInclude(g => g.Members)
                .ToListAsync();
        }

        public void UpdateEvent(Event UpdateEvent)
        {
            _context.Events.Update(UpdateEvent);
            _context.SaveChanges();
        }
    }
}
