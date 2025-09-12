using Application.Dtos.NotificationDtos;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllByUserAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetUnreadByUserAsync(int userId);
        Task<NotificationDto> CreateAsync(NotificationCreateDto dto);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(int userId);
        Task DeleteAsync(int id);
    }
}
