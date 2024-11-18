using Application.DTOs;
using Application.DTOs.Event;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<EventDto?> AddEvent(EventCreateDto eventDto)
        {
            var user = await _userRepository.GetUserById(eventDto.CreatedByUserId);

            if (user == null)
            {
                throw new Exception($"User with ID {eventDto.CreatedByUserId} not found.");
            }

            var eventEntity = new Event
            {
                EventName = eventDto.EventName,
                Location = eventDto.Location,
                Date = eventDto.Date,
                CreatedByUserId = eventDto.CreatedByUserId
            };

            var newGroup = new Group
            {
                ManagedByUserId = eventDto.CreatedByUserId,
                Members = new List<AppUser> { user } 
            };

            eventEntity.Group = newGroup;

            await _eventRepository.Add(eventEntity);

            var eventResponse = _mapper.Map<EventDto>(eventEntity);

            eventResponse.GroupId = newGroup.Id;

            return eventResponse;
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            var eventToDelete = await _eventRepository.GetEvent(eventId);

            if (eventToDelete == null)
            {
                return false;
            }

            _eventRepository.DeleteEvent(eventToDelete);

            return true;
        }

        public async Task<IEnumerable<EventDto>> GetEvents()
        {
            var events = await _eventRepository.GetEvents();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEvent(int id)
        {
            var eventEntity = await _eventRepository.GetEvent(id);

            if (eventEntity == null)
            {
                return null;
            }

            var eventDto = _mapper.Map<EventDto>(eventEntity);

            return eventDto;
        }

        public async Task<bool> UpdateEvent(EventUpdateDto eventUpdateDto)
        {
            var eventToUpdate = await _eventRepository.GetEvent(eventUpdateDto.Id);

            if (eventToUpdate == null)
            {
                return false;
            }

            _mapper.Map(eventUpdateDto, eventToUpdate);
            _eventRepository.UpdateEvent(eventToUpdate);

            return true;
        }
    }
}
