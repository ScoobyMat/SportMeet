using Application.Dtos.EventDtos;
using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAllGroupsAsync();
        Task<GroupDto?> GetGroupByIdAsync(int groupId);
        Task<GroupDto?> GetGroupByEventIdAsync(int eventId);
        Task<GroupDto> CreateGroupAsync(GroupCreateDto groupCreateDto);
    }
}