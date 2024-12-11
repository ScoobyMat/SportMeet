using Application.Dtos.GroupDtos;

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