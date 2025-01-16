using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventDtos
{
    public class EventDto : IMap
    {
        public required int Id { get; set; }
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string SportType { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeSpan Time { get; set; }
        public required string CreatedByUser { get; set; }
        public required int MaxMembers { get; set; }
        public required int CurrentMembers { get; set; }
        public required int GroupId { get; set; }
        public string? PhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, EventDto>()
                .ForMember(dest => dest.MaxMembers, opt => opt.MapFrom(src => src.MaxParticipants))
                .ForMember(dest => dest.CurrentMembers, opt => opt.MapFrom(src => src.Group.Members.Count))
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}"))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}
