using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<bool> AddMemberToGroupByUserId(int groupId, int userId);
        Task<bool> AddMemberToGroupByEmail(int groupId, string email); 
        Task<bool> RemoveMemberFromGroup(int groupId, int userId);
    }
}
