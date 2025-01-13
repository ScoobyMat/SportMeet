using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByGroupIdAsync(int groupId);
        Task AddMessageAsync(Message message);
    }
}
