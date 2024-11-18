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
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context; 
        }

        public async Task Add(Group group)
        {
            await _context.Groups.AddAsync(group);
            _context.SaveChanges();
        }

        public void DeleteGroup(Group group)
        {
           _context.Groups.Remove(group);
           _context.SaveChanges();
        }

        public async Task<Group?> GetGroup(int id)
        {
            return await _context.Groups
                .Include(g => g.Event)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            return await _context.Groups
                .Include(g => g.Event)
                .ToListAsync();
        }

        public void UpdateGroup(Group group)
        {
            _context.Groups.Update(group);
            _context.SaveChanges();
        }

    }
}
