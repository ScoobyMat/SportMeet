using Application.Dtos.EventAttendeeDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventAttendeeService : IEventAttendeeService
    {
        private readonly IEventAttendeeRepository _attendeeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventAttendeeService(IEventAttendeeRepository attendeeRepository,IEventRepository eventRepository, IMapper mapper)
        {
            _attendeeRepository = attendeeRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }


        public async Task<EventAttendeeDto> AddAttendeeAsync(int eventId, int userId)
        {
            var existing = await _attendeeRepository.GetByEventAndUserAsync(eventId, userId);
            if (existing != null)
            {
                throw new InvalidOperationException("User is already registered for the event.");
            }

            var dto = new EventAttendeeCreateDto
            {
                EventId = eventId,
                UserId = userId
            };

            var attendee = _mapper.Map<EventAttendee>(dto);
            attendee.JoinedAt = DateTime.UtcNow;

            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }
            attendee.Role = ev.CreatedByUserId == userId ? "Owner" : "Participant";

            await _attendeeRepository.AddAsync(attendee);
            return _mapper.Map<EventAttendeeDto>(attendee);
        }




        public async Task DeleteAttendeeAsync(int attendeeId)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(attendeeId);
            if (attendee == null)
                throw new KeyNotFoundException("Attendee not found.");

            await _attendeeRepository.DeleteAsync(attendee);
        }

        public async Task<EventAttendeeDto?> GetByEventAndUserAsync(int eventId, int userId)
        {
            var attendee = await _attendeeRepository.GetByEventAndUserAsync(eventId, userId);
            return attendee == null ? null : _mapper.Map<EventAttendeeDto>(attendee);
        }

        public async Task<IEnumerable<EventAttendeeDto>> GetByEventIdAsync(int eventId)
        {
            var attendees = await _attendeeRepository.GetByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<EventAttendeeDto>>(attendees);
        }
    }
}
