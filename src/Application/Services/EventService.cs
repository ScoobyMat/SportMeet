using Application.Dtos.EventDtos;
using Application.Dtos.GroupDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository,IGroupRepository groupRepository,IGroupService groupService,IMapper mapper)
        {
            _eventRepository = eventRepository;
            _groupRepository = groupRepository;
            _groupService = groupService;
            _mapper = mapper;
        }

        public async Task<EventDto?> AddEventAsync(EventCreateDto eventDto)
        {
            var newEvent = _mapper.Map<Event>(eventDto);
            await _eventRepository.AddAsync(newEvent);

            var GroupCreateDto = new GroupCreateDto
            {
                EventId = newEvent.Id,
                CreatedByUserId = newEvent.CreatedByUserId,
            };

            var groupDto = await _groupService.CreateGroupAsync(GroupCreateDto);

            var createdEvent = await _eventRepository.GetByIdAsync(newEvent.Id);
            var eventDtoMapped = _mapper.Map<EventDto>(createdEvent);

            return eventDtoMapped;
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
            var eventToUpdate = await _eventRepository.GetByIdAsync(eventUpdateDto.Id);
            if (eventToUpdate == null)
            {
                return false;
            }

            _mapper.Map(eventUpdateDto, eventToUpdate);
            await _eventRepository.UpdateAsync(eventToUpdate);
            return true;
        }
    }
}
