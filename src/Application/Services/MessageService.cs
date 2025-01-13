using Application.Dtos.MessageDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageService(
            IMessageRepository messageRepository,
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<MessageDto>> GetMessagesForGroupAsync(int groupId)
        {
            var messages = await _messageRepository.GetMessagesByGroupIdAsync(groupId);
            return _mapper.Map<List<MessageDto>>(messages);
        }

        public async Task<MessageDto> AddMessageAsync(CreateMessageDto createMessageDto)
        {
            if (string.IsNullOrEmpty(createMessageDto.Content) || createMessageDto.GroupId <= 0)
            {
                throw new ArgumentException("Invalid message content or group ID.");
            }

            var message = _mapper.Map<Message>(createMessageDto);
            message.SentAt = DateTime.UtcNow;

            await _messageRepository.AddMessageAsync(message);

            return _mapper.Map<MessageDto>(message);
        }

        public async Task<MessageDto> GetMessageByIdAsync(int messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                throw new KeyNotFoundException($"Message not found.");
            }

            return _mapper.Map<MessageDto>(message);
        }
    }
}
