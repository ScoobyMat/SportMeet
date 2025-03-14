using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventAttendeeDtos
{
    public class EventAttendeeCreateDto : IMap
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventAttendeeCreateDto, EventAttendee>();
        }
    }
}
