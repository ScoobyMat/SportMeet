using Application.Dtos.NotificationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllByUserAsync(int userId);
        Task<NotificationDto> GetByIdAsync(int id);
        Task<NotificationDto> CreateAsync(NotificationCreateDto dto);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
