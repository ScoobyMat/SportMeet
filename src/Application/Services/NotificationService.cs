using Application.Dtos.NotificationDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllByUserAsync(int userId)
        {
            var notifs = await _repo.GetAllByUserAsync(userId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifs);
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadByUserAsync(int userId)
        {
            var notifs = await _repo.GetAllByUserAsync(userId);
            var unreadNotifs = notifs.Where(n => !n.IsRead);
            return _mapper.Map<IEnumerable<NotificationDto>>(unreadNotifs);
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
            return _mapper.Map<NotificationDto>(entity);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notif = await _repo.GetByIdAsync(id);
            if (notif == null)
                throw new KeyNotFoundException("Notification not found");

            await _repo.DeleteAsync(notif);
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifs = await _repo.GetAllByUserAsync(userId);
            var unreadNotifs = notifs.Where(n => !n.IsRead);

            foreach (var notif in unreadNotifs)
            {
                await _repo.DeleteAsync(notif);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var notif = await _repo.GetByIdAsync(id);
            if (notif == null)
                throw new KeyNotFoundException("Notification not found");

            await _repo.DeleteAsync(notif);
        }
    }
}
