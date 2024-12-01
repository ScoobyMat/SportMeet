using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<GroupMember?> GetMemberByIdAsync(int memberId);
        Task AddMemberAsync(GroupMember groupMember);
        Task UpdateMemberAsync(GroupMember groupMember);
        Task RemoveMemberAsync(int groupId, int userId);
    }
}
