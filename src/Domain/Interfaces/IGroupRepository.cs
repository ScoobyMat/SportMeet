using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroups();
        Task<Group?> GetGroup(int id);
        Task Add(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);
    }
}
