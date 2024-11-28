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
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<EventDto?> AddEventAsync(EventCreateDto eventDto)
        {
            var user = await _userRepository.GetByIdAsync(eventDto.CreatedByUserId);
            if (user == null)
            {
                return null;
            }

            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.CreatedByUserId = eventDto.CreatedByUserId;

            var newGroup = new Group
            {
                ManagedByUserId = eventDto.CreatedByUserId,
                Members = new List<GroupMember>
                    {
                        new GroupMember { UserId = eventDto.CreatedByUserId }
                    }
            };

            eventEntity.Group = newGroup;

            await _eventRepository.AddAsync(eventEntity);

            var eventResponse = _mapper.Map<EventDto>(eventEntity);
            eventResponse.GroupId = newGroup.Id;

            return eventResponse;
        }


        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(eventId);

            if (eventToDelete == null)
            {
                return false;
            }

            await _eventRepository.DeleteAsync(eventToDelete);
            return true;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);

            if (eventEntity == null)
            {
                return null;
            }

            var eventDto = _mapper.Map<EventDto>(eventEntity);

            return eventDto;
        }

        public async Task<bool> UpdateEventAsync(EventUpdateDto eventUpdateDto)
        {
            var eventToUpdat = await _eventRepository.GetByIdAsync(eventUpdateDto.Id);
            if (eventToUpdat == null)
            {
                return false;
            }

            _mapper.Map(eventUpdateDto, eventToUpdat);
            await _eventRepository.UpdateAsync(eventToUpdat);
            return true;
        }
    }
}
