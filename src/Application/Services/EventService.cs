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
        private readonly IGroupService _groupService;
        private readonly IPhotoService _photoService;
        private readonly IGeoCodingService _geoCodingService;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IGroupService groupService, IPhotoService photoService, IGeoCodingService geoCodingService, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _groupService = groupService;
            _photoService = photoService;
            _geoCodingService = geoCodingService;
            _mapper = mapper;
        }

        public async Task<EventDto?> AddEventAsync(EventCreateDto eventDto)
        {
            var newEvent = _mapper.Map<Event>(eventDto);

            var geoCodingResult = await _geoCodingService.GetCoordinatesAsync(newEvent);
            if (geoCodingResult == null)
            {
                throw new InvalidOperationException("Failed to retrieve coordinates for the event location.");
            }

            newEvent = geoCodingResult;


            if (eventDto.Photo != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(eventDto.Photo, "event");

                newEvent.PhotoUrl = uploadResult.Url.ToString();
                newEvent.PhotoPublicId = uploadResult.PublicId;
            }

            await _eventRepository.AddAsync(newEvent);

            var groupCreateDto = new GroupCreateDto
            {
                EventId = newEvent.Id,
                CreatedByUserId = newEvent.CreatedByUserId,
                MaxMembers = newEvent.MaxParticipants,
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

            if (eventToDelete.PhotoPublicId != null)
            {
                await _photoService.DeletePhotoAsync(eventToDelete.PhotoPublicId);

                eventToDelete.PhotoUrl = null;
                eventToDelete.PhotoPublicId = null;

                await _eventRepository.UpdateAsync(eventToDelete);
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

        public async Task<List<EventDto>> GetFilteredEventsAsync(string? location, string? sportType, DateOnly? startDate, DateOnly? endDate)
        {
            var events = await _eventRepository.GetFilteredEventsAsync(location, sportType, startDate, endDate);
            if (events == null || !events.Any())
            {
                return new List<EventDto>();
            }

            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<IEnumerable<EventDto>> GetUpcomingEventsForUserAsync(int userId)
        {
            var events = await _eventRepository.GetUpcomingEventsForUserAsync(userId);

            if (events == null || !events.Any())
            {
                return new List<EventDto>();
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

            if (eventUpdateDto.Photo != null)
            {
                if (eventToUpdate.PhotoPublicId != null)
                {
                    await _photoService.DeletePhotoAsync(eventToUpdate.PhotoPublicId);
                }

                var uploadResult = await _photoService.AddPhotoAsync(eventUpdateDto.Photo, "event");

                eventToUpdate.PhotoUrl = uploadResult.Url.ToString();
                eventToUpdate.PhotoPublicId = uploadResult.PublicId;
            }

            if (!string.IsNullOrWhiteSpace(eventUpdateDto.Address) || !string.IsNullOrWhiteSpace(eventUpdateDto.City))
            {
                eventToUpdate.Address = eventUpdateDto.Address;
                eventToUpdate.City = eventUpdateDto.City;

                var geoCodedEvent = await _geoCodingService.GetCoordinatesAsync(eventToUpdate);
                if (geoCodedEvent != null)
                {
                    eventToUpdate.Latitude = geoCodedEvent.Latitude;
                    eventToUpdate.Longitude = geoCodedEvent.Longitude;
                }
                else
                {
                    throw new InvalidOperationException("Could not determine coordinates for the given address.");
                }
            }

            _mapper.Map(eventUpdateDto, eventToUpdate);
            await _eventRepository.UpdateAsync(eventToUpdate);
        }

    }
}
