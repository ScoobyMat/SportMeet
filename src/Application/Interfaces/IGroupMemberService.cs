using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupMemberService
    {
        Task AddMemberAsync(AddMemberDto addMemberDto);
        Task<bool> RemoveMemberAsync(RemoveMemberDto removeMemberDto);
        Task<bool> UpdateManagerAsync(GroupUpdateDto groupUpdateDto);
    }
}
