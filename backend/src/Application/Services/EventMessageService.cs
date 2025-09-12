using Application.Dtos.EventMessageDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventMessageService : IEventMessageService
    {
        private readonly IEventMessageRepository _eventMessageRepository;
        private readonly IMapper _mapper;

        public EventMessageService(IEventMessageRepository eventMessageRepository, IMapper mapper)
        {
            _eventMessageRepository = eventMessageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventMessageDto>> GetMessagesForEventAsync(int eventId)
        {
            var messages = await _eventMessageRepository.GetAllByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<EventMessageDto>>(messages);
        }

        public async Task<EventMessageDto> SendMessageAsync(EventMessageCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Message content cannot be empty.");

            var message = _mapper.Map<EventMessage>(dto);
            message.Created = DateTime.UtcNow;
            await _eventMessageRepository.AddAsync(message);

            var savedMessage = await _eventMessageRepository.GetByIdAsync(message.Id);

            return _mapper.Map<EventMessageDto>(savedMessage);
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var msg = await _eventMessageRepository.GetByIdAsync(messageId);
            if (msg == null)
            {
                throw new KeyNotFoundException("Message not found");
            }

            await _eventMessageRepository.DeleteAsync(msg);
        }
    }
}
