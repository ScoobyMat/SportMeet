using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task AddUser(AppUser user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Delete(AppUser user)
        {
            _context.Users.Remove(user);
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.ProfilePhoto)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.ProfilePhoto)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.ProfilePhoto)
                .ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> UserExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
