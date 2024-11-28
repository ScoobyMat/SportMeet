using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Group group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Group group)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Event)
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups
                .Include(g => g.Event)
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .ToListAsync();
        }

        public async Task AddMemberToGroupAsync(int groupId, int userId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            var user = await _context.Users.FindAsync(userId);

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                Group = group,
                User = user
            };

            group.Members.Add(groupMember);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberFromGroupAsync(int groupId, int userId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            var groupMember = group?.Members.FirstOrDefault(m => m.UserId == userId);

            if (groupMember != null)
            {
                group.Members.Remove(groupMember);
                await _context.SaveChangesAsync();
            }
        }
    }
}
