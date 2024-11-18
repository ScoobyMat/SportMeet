using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.Event
{
    public class EventUpdateDto : IMap
    {
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventUpdateDto, EventDto>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.EventName))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
        }
    }
}
