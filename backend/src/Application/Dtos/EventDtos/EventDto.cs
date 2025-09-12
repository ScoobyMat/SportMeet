using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventDtos
{
    public class EventDto : IMap
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string EventName { get; set; } = null!;
        public string? Description { get; set; }
        public string SportType { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }

        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }

        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = null!;

        public int MaxParticipants { get; set; }
        public int CurrentMembers { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, EventDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.LastModified))
                .ForMember(dest => dest.CurrentMembers, opt => opt.MapFrom(src => src.Attendees != null ? src.Attendees.Count : 0))
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.FirstName + " " + src.CreatedByUser.LastName));
        }
    }
}
