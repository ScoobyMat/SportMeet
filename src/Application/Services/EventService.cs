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

        public EventService(IEventRepository eventRepository, IGroupRepository groupRepository, IGroupService groupService, IMapper mapper)
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

            var groupCreateDto = new GroupCreateDto
            {
                EventId = newEvent.Id,
                CreatedByUserId = newEvent.CreatedByUserId,
            };

            try
            {
                await _groupService.CreateGroupAsync(groupCreateDto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create associated group.", ex);
            }

            var createdEvent = await _eventRepository.GetByIdAsync(newEvent.Id);
            if (createdEvent == null)
            {
                throw new InvalidOperationException("Failed to retrieve the newly created event.");
            }

            var eventDtoMapped = _mapper.Map<EventDto>(createdEvent);
            return eventDtoMapped;
        }

        public async Task DeleteEventAsync(int eventId)
        {
            var eventToDelete = await _eventRepository.GetByIdAsync(eventId);
            if (eventToDelete == null)
            {
                throw new KeyNotFoundException($"Event not found.");
            }

            await _eventRepository.DeleteAsync(eventToDelete);
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            if (events == null || !events.Any())
            {
                throw new InvalidOperationException("No events were found.");
            }

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(id);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event not found.");
            }

            var eventDto = _mapper.Map<EventDto>(eventEntity);
            return eventDto;
        }

        public async Task<List<EventDto>> GetFilteredEventsAsync(string? location, DateOnly? startDate, DateOnly? endDate)
        {
            var events = await _eventRepository.GetFilteredEventsAsync(location, startDate, endDate);
            if (events == null || !events.Any())
            {
                throw new InvalidOperationException("No events matched the given filter criteria.");
            }

            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<IEnumerable<EventDto>> GetUpcomingEventsForUserAsync(int userId)
        {
            var events = await _eventRepository.GetUpcomingEventsForUserAsync(userId);
            if (events == null || !events.Any())
            {
                throw new InvalidOperationException("No upcoming events found for the user.");
            }

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task UpdateEventAsync(EventUpdateDto eventUpdateDto)
        {
            var eventToUpdate = await _eventRepository.GetByIdAsync(eventUpdateDto.Id);
            if (eventToUpdate == null)
            {
                throw new InvalidOperationException($"Event not found.");
            }

            _mapper.Map(eventUpdateDto, eventToUpdate);
            await _eventRepository.UpdateAsync(eventToUpdate);
        }
    }
}
