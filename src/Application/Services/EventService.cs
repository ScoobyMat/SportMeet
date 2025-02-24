using Application.Dtos.EventDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventAttendeeRepository _attendeeRepository;
        private readonly IMapper _mapper;

        public EventService(
            IEventRepository eventRepository,
            IUserRepository userRepository,
            IEventAttendeeRepository attendeeRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _attendeeRepository = attendeeRepository;
            _mapper = mapper;
        }

        public async Task<EventDto> CreateAsync(EventCreateDto dto)
        {
            var creator = await _userRepository.GetByIdAsync(dto.CreatedByUserId);
            if (creator == null) throw new KeyNotFoundException("Creator user not found.");

            var ev = _mapper.Map<Event>(dto);
            ev.Created = DateTime.UtcNow;

            await _eventRepository.AddAsync(ev);

            var attendee = new EventAttendee
            {
                EventId = ev.Id,
                UserId = dto.CreatedByUserId,
                Role = "Owner",
            };
            await _attendeeRepository.AddAsync(attendee);

            return _mapper.Map<EventDto>(ev);
        }

        public async Task<EventDto> UpdateAsync(EventUpdateDto dto)
        {
            var ev = await _eventRepository.GetByIdAsync(dto.Id);
            if (ev == null) throw new KeyNotFoundException("Event not found.");

            _mapper.Map(dto, ev);
            ev.LastModified = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(ev);

            return _mapper.Map<EventDto>(ev);
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null) throw new KeyNotFoundException("Event not found.");

            return _mapper.Map<EventDto>(ev);
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task DeleteAsync(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null) throw new KeyNotFoundException("Event not found.");

            await _eventRepository.DeleteAsync(ev);
        }

        public async Task JoinEventAsync(int eventId, int userId)
        {
            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null) throw new KeyNotFoundException("Event not found.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            var existing = (await _attendeeRepository.GetByEventIdAsync(eventId))
                .FirstOrDefault(a => a.UserId == userId);

            if (existing != null)
            {
                return;
            }

            var attendee = new EventAttendee
            {
                EventId = eventId,
                UserId = userId,
                Role = "Participant",
            };
            await _attendeeRepository.AddAsync(attendee);
        }

        public async Task<List<EventDto>> GetUpcomingEventsForUserAsync(int userId)
        {
            var events = await _eventRepository.GetUpcomingEventsForUserAsync(userId);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetManagedEventsAsync(int userId)
        {
            var events = await _eventRepository.GetManagedEventsAsync(userId);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> SearchEventsAsync(EventSearchDto searchDto)
        {
            var events = await _eventRepository.SearchEventsAsync(
                searchDto.City,
                searchDto.From,
                searchDto.To,
                searchDto.SportType
            );
            return _mapper.Map<List<EventDto>>(events);
        }

    }
}
