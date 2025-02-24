using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DataContext _context;

        public FriendshipRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Friendship?> GetByIdAsync(int friendshipId)
        {
            return await _context.Friendships
                .Include(f => f.Requestor)
                .Include(f => f.Addressee)
                .FirstOrDefaultAsync(f => f.Id == friendshipId);
        }

        public async Task<Friendship?> GetRelationAsync(int userAId, int userBId)
        {
            return await _context.Friendships
                .Where(f =>
                    (f.RequestorId == userAId && f.AddresseeId == userBId) ||
                    (f.RequestorId == userBId && f.AddresseeId == userAId)
                )
                .FirstOrDefaultAsync();
        }

        public async Task<List<Friendship>> GetAcceptedFriendshipsForUserAsync(int userId)
        {
            return await _context.Friendships
                .Where(f => f.Status == FriendshipStatus.Accepted
                      && (f.RequestorId == userId || f.AddresseeId == userId))
                .Include(f => f.Requestor)
                .Include(f => f.Addressee)
                .ToListAsync();
        }

        public async Task AddAsync(Friendship friendship)
        {
            await _context.Friendships.AddAsync(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Friendship friendship)
        {
            _context.Friendships.Update(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Friendship friendship)
        {
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
        }
    }
}
