using Application.Dtos.EventMessageDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventMessageService : IEventMessageService
    {
        private readonly IEventMessageRepository _repo;
        private readonly IMapper _mapper;

        public EventMessageService(IEventMessageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventMessageDto>> GetMessagesForEventAsync(int eventId)
        {
            var messages = await _repo.GetAllByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<EventMessageDto>>(messages);
        }

        public async Task<EventMessageDto> GetMessageByIdAsync(int id)
        {
            var msg = await _repo.GetByIdAsync(id);
            if (msg == null) throw new KeyNotFoundException("Message not found");

            return _mapper.Map<EventMessageDto>(msg);
        }

        public async Task<EventMessageDto> CreateMessageAsync(EventMessageCreateDto dto)
        {
            var msg = new EventMessage
            {
                EventId = dto.EventId,
                SenderId = dto.SenderId,
                Content = dto.Content,
                Created = DateTime.UtcNow
            };

            await _repo.AddAsync(msg);

            return _mapper.Map<EventMessageDto>(msg);
        }

        public async Task DeleteMessageAsync(int id)
        {
            var msg = await _repo.GetByIdAsync(id);
            if (msg == null) throw new KeyNotFoundException("Message not found");

            await _repo.DeleteAsync(msg);
        }
    }
}