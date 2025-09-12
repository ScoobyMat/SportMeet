using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventMessageRepository : IEventMessageRepository
    {
        private readonly DataContext _context;

        public EventMessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EventMessage?> GetByIdAsync(int id)
        {
            return await _context.EventMessages
                .Include(em => em.Sender)
                .Include(em => em.Event)
                .FirstOrDefaultAsync(em => em.Id == id);
        }

        public async Task<IEnumerable<EventMessage>> GetAllByEventIdAsync(int eventId)
        {
            return await _context.EventMessages
                .Include(em => em.Sender)
                .Where(em => em.EventId == eventId)
                .OrderBy(em => em.Created)
                .ToListAsync();
        }

        public async Task AddAsync(EventMessage message)
        {
            await _context.EventMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventMessage message)
        {
            _context.EventMessages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EventMessage message)
        {
            _context.EventMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
