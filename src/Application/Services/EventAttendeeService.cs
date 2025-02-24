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
        private readonly IMapper _mapper;

        public EventAttendeeService(IEventAttendeeRepository attendeeRepository, IMapper mapper)
        {
            _attendeeRepository = attendeeRepository;
            _mapper = mapper;
        }

        public async Task<EventAttendeeDto?> GetByIdAsync(int id)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            if (attendee == null) return null;

            return _mapper.Map<EventAttendeeDto>(attendee);
        }

        public async Task<IEnumerable<EventAttendeeDto>> GetByEventIdAsync(int eventId)
        {
            var attendees = await _attendeeRepository.GetByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<EventAttendeeDto>>(attendees);
        }

        public async Task<EventAttendeeDto> AddAttendeeAsync(EventAttendeeCreateDto dto)
        {
            var entity = new EventAttendee
            {
                EventId = dto.EventId,
                UserId = dto.UserId,
                Role = dto.Role,
                Created = DateTime.UtcNow
            };

            await _attendeeRepository.AddAsync(entity);

            return _mapper.Map<EventAttendeeDto>(entity);
        }

        public async Task UpdateAttendeeAsync(EventAttendeeUpdateDto dto)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(dto.Id);
            if (attendee == null) throw new KeyNotFoundException("Attendee not found");

            if (dto.Role != null)
            {
                attendee.Role = dto.Role;
                attendee.LastModified = DateTime.UtcNow;
                await _attendeeRepository.UpdateAsync(attendee);
            }
        }

        public async Task DeleteAttendeeAsync(int id)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            if (attendee == null) throw new KeyNotFoundException("Attendee not found");

            await _attendeeRepository.DeleteAsync(attendee);
        }
    }
}
