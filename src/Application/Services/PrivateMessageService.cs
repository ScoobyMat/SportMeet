using Application.Dtos.PrivateMessageDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IPrivateMessageRepository _repo;
        private readonly IMapper _mapper;

        public PrivateMessageService(IPrivateMessageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrivateMessageDto>> GetConversationAsync(int userAId, int userBId)
        {
            var messages = await _repo.GetConversationAsync(userAId, userBId);
            return messages.Select(m => new PrivateMessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                RecipientId = m.RecipientId,
                Content = m.Content,
                Created = m.Created
            });
            // lub _mapper.Map<IEnumerable<PrivateMessageDto>>(messages)
        }

        public async Task<PrivateMessageDto> SendMessageAsync(PrivateMessageCreateDto dto)
        {
            var message = new PrivateMessage
            {
                SenderId = dto.SenderId,
                RecipientId = dto.RecipientId,
                Content = dto.Content,
                Created = DateTime.UtcNow
            };

            await _repo.AddAsync(message);

            return new PrivateMessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                RecipientId = message.RecipientId,
                Content = message.Content,
                Created = message.Created,
            };
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var msg = await _repo.GetByIdAsync(messageId);
            if (msg == null) throw new KeyNotFoundException("Message not found");

            await _repo.DeleteAsync(msg);
        }
    }
}
