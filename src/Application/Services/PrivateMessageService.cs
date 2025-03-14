using Application.Dtos.PrivateMessageDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IPrivateMessageRepository _privateMessageRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public PrivateMessageService(IPrivateMessageRepository privateMessageRepository, INotificationRepository notificationRepository,IMapper mapper)
        {
            _privateMessageRepository = privateMessageRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrivateMessageDto>> GetConversationAsync(int userAId, int userBId)
        {
            var messages = await _privateMessageRepository.GetConversationAsync(userAId, userBId);
            return _mapper.Map<IEnumerable<PrivateMessageDto>>(messages);
        }

        public async Task<PrivateMessageDto> SendMessageAsync(PrivateMessageCreateDto dto)
        {
            var message = _mapper.Map<PrivateMessage>(dto);

            await _privateMessageRepository.AddAsync(message);

            var savedMessage = await _privateMessageRepository.GetByIdAsync(message.Id);

            var notification = new Notification
            {
                UserId = dto.RecipientId,
                Type = "NewMessage",
                Message = $"Nowa wiadomość od użytkownika {dto.SenderId}",
                IsRead = false,
                Created = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            return _mapper.Map<PrivateMessageDto>(savedMessage);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var msg = await _privateMessageRepository.GetByIdAsync(messageId);
            if (msg == null) throw new KeyNotFoundException("Message not found");

            await _privateMessageRepository.DeleteAsync(msg);
        }
    }
}
