using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<Friendship?> GetByIdAsync(int friendshipId);
        Task<Friendship?> GetRelationAsync(int userAId, int userBId);
        Task<List<Friendship>> GetAcceptedFriendshipsForUserAsync(int userId);
        Task<List<Friendship>> GetSentInvitationsAsync(int userId);
        Task<List<Friendship>> GetReceivedInvitationsAsync(int userId);
        Task AddAsync(Friendship friendship);
        Task UpdateAsync(Friendship friendship);
        Task DeleteAsync(Friendship friendship);
    }
}
