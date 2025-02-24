using Application.Dtos.NotificationDtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllByUserAsync(int userId)
        {
            var notifs = await _repo.GetAllByUserAsync(userId);
            return notifs.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                Message = n.Message,
                IsRead = n.IsRead,
                Created = n.Created
            });
        }

        public async Task<NotificationDto> GetByIdAsync(int id)
        {
            var notif = await _repo.GetByIdAsync(id);
            if (notif == null) throw new KeyNotFoundException("Notification not found");

            return new NotificationDto
            {
                Id = notif.Id,
                UserId = notif.UserId,
                Type = notif.Type,
                Message = notif.Message,
                IsRead = notif.IsRead,
                Created = notif.Created
            };
        }

        public async Task<NotificationDto> CreateAsync(NotificationCreateDto dto)
        {
            var entity = new Notification
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Message = dto.Message,
                IsRead = false,
                Created = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);

            return new NotificationDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Type = entity.Type,
                Message = entity.Message,
                IsRead = entity.IsRead,
                Created = entity.Created
            };
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notif = await _repo.GetByIdAsync(id);
            if (notif == null) throw new KeyNotFoundException("Notification not found");

            notif.IsRead = true;
            await _repo.UpdateAsync(notif);
        }

        public async Task DeleteAsync(int id)
        {
            var notif = await _repo.GetByIdAsync(id);
            if (notif == null) throw new KeyNotFoundException("Notification not found");

            await _repo.DeleteAsync(notif);
        }
    }
}
