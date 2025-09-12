using Application.Dtos.EventDtos;
using Application.Dtos.EventAttendeeDtos;
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
        private readonly IPhotoService _photoService;
        private readonly IGeoCodingService _geoCodingService;
        private readonly IEventAttendeeService _eventAttendeeService;
        private readonly IMapper _mapper;

        public EventService(
            IEventRepository eventRepository,
            IUserRepository userRepository,
            IPhotoService photoService,
            IGeoCodingService geoCodingService,
            IEventAttendeeService eventAttendeeService,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _photoService = photoService;
            _geoCodingService = geoCodingService;
            _eventAttendeeService = eventAttendeeService;
            _mapper = mapper;
        }

        public async Task<EventDto> CreateAsync(EventCreateDto dto)
        {
            var creator = await _userRepository.GetByIdAsync(dto.CreatedByUserId);
            if (creator == null)
                throw new KeyNotFoundException("Creator user not found.");

            var newEvent = _mapper.Map<Event>(dto);
            newEvent.Created = DateTime.UtcNow;
            newEvent.Status = EventStatus.Coming;

            var geoCodingResult = await _geoCodingService.GetCoordinatesAsync(newEvent);
            if (geoCodingResult == null)
                throw new InvalidOperationException("Failed to retrieve coordinates for the event location.");
            newEvent = geoCodingResult;

            if (dto.Photo != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(dto.Photo, "event");
                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                newEvent.PhotoUrl = uploadResult.Url.ToString();
                newEvent.PhotoPublicId = uploadResult.PublicId;
            }

            await _eventRepository.AddAsync(newEvent);

            await _eventAttendeeService.AddAttendeeAsync(newEvent.Id, newEvent.CreatedByUserId);

            return _mapper.Map<EventDto>(newEvent);
        }



        public async Task<EventDto> UpdateAsync(EventUpdateDto dto)
        {
            var ev = await _eventRepository.GetByIdAsync(dto.Id);
            if (ev == null)
                throw new KeyNotFoundException("Event not found.");

            if (dto.Photo != null)
            {
                if (!string.IsNullOrEmpty(ev.PhotoPublicId))
                {
                    await _photoService.DeletePhotoAsync(ev.PhotoPublicId);
                }
                var uploadResult = await _photoService.AddPhotoAsync(dto.Photo, "event");
                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                ev.PhotoUrl = uploadResult.Url.ToString();
                ev.PhotoPublicId = uploadResult.PublicId;
            }

            if (!string.IsNullOrWhiteSpace(dto.Address) || !string.IsNullOrWhiteSpace(dto.City))
            {
                ev.Address = !string.IsNullOrWhiteSpace(dto.Address) ? dto.Address : ev.Address;
                ev.City = !string.IsNullOrWhiteSpace(dto.City) ? dto.City : ev.City;

                var geoCodedEvent = await _geoCodingService.GetCoordinatesAsync(ev);
                if (geoCodedEvent != null)
                {
                    ev.Latitude = geoCodedEvent.Latitude;
                    ev.Longitude = geoCodedEvent.Longitude;
                }
                else
                {
                    throw new InvalidOperationException("Could not determine coordinates for the given address.");
                }
            }

            _mapper.Map(dto, ev);
            ev.LastModified = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(ev);
            return _mapper.Map<EventDto>(ev);
        }




        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null)
                throw new KeyNotFoundException("Event not found.");

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
            if (ev == null)
                throw new KeyNotFoundException("Event not found.");

            if (!string.IsNullOrEmpty(ev.PhotoPublicId))
            {
                await _photoService.DeletePhotoAsync(ev.PhotoPublicId);
                ev.PhotoUrl = null;
                ev.PhotoPublicId = null;
                await _eventRepository.UpdateAsync(ev);
            }

            ev.Status = EventStatus.Cancelled;
            await _eventRepository.DeleteAsync(ev);
        }

        public async Task<List<EventDto>> GetUpcomingEventsAsync()
        {
            var events = await _eventRepository.GetUpcomingEventsAsync();
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetPastEventsAsync()
        {
            var events = await _eventRepository.GetPastEventsAsync();
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetUpcomingEventsForUserAsync(int userId)
        {
            var events = await _eventRepository.GetUpcomingEventsForUserAsync(userId);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetPastEventsForUserAsync(int userId)
        {
            var events = await _eventRepository.GetPastEventsForUserAsync(userId);
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

        public async Task<List<EventDto>> GetManagedEventsAsync(int userId)
        {
            var events = await _eventRepository.GetManagedEventsAsync(userId);
            return _mapper.Map<List<EventDto>>(events);
        }
    }
}
