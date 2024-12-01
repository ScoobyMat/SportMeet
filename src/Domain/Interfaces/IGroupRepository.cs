using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group?> GetByIdAsync(int id);
        Task<Group?> GetGroupByEventIdAsync(int eventId);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Group group);
    }
}
