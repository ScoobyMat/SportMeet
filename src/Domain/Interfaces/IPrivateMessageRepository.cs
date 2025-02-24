using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPrivateMessageRepository
    {
        Task<PrivateMessage?> GetByIdAsync(int id);
        Task<IEnumerable<PrivateMessage>> GetConversationAsync(int userAId, int userBId);
        Task AddAsync(PrivateMessage message);
        Task UpdateAsync(PrivateMessage message);
        Task DeleteAsync(PrivateMessage message);
    }
}
