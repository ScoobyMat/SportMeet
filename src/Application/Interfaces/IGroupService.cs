using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task AddMemberToGroupByEmailAsync(int groupId, string email);
        Task AddMemberToGroupByUserIdAsync(int groupId, int userId);
        Task RemoveMemberFromGroupAsync(int groupId, int userId);
    }
}
