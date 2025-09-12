using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly DataContext _context;
        public PrivateMessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PrivateMessage?> GetByIdAsync(int id)
        {
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Recipient)
                .FirstOrDefaultAsync(pm => pm.Id == id);
        }

        public async Task<IEnumerable<PrivateMessage>> GetConversationAsync(int userAId, int userBId)
        {
            return await _context.PrivateMessages
                .Include(pm => pm.Sender)
                .Include(pm => pm.Recipient)
                .Where(pm =>
                    (pm.SenderId == userAId && pm.RecipientId == userBId) ||
                    (pm.SenderId == userBId && pm.RecipientId == userAId)
                )
                .OrderBy(pm => pm.Created)
                .ToListAsync();
        }

        public async Task AddAsync(PrivateMessage message)
        {
            await _context.PrivateMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PrivateMessage message)
        {
            _context.PrivateMessages.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PrivateMessage message)
        {
            _context.PrivateMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
