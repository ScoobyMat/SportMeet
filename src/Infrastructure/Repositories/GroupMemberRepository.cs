using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly DataContext _context;

        public GroupMemberRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddMemberAsync(GroupMember groupMember)
        {
            await _context.GroupMembers.AddAsync(groupMember);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(GroupMember groupMember)
        {

            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
        }

        public async Task<GroupMember?> GetMemberByIdAsync(int memberId)
        {
            return await _context.GroupMembers.FirstOrDefaultAsync(m => m.Id == memberId);
        }

        public async Task UpdateMemberAsync(GroupMember groupMember)
        {
            _context.GroupMembers.Update(groupMember);
            await _context.SaveChangesAsync();
        }
    }
}
