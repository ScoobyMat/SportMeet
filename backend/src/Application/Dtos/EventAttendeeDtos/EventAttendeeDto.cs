using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventAttendeeDtos
{
    public class EventAttendeeDto : IMap
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
        public DateTime JoinedAt { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventAttendee, EventAttendeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : null))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User != null ? src.User.LastName : null))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.User != null ? src.User.PhotoUrl : null));
        }
    }
}