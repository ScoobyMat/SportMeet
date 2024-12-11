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
        public required DateOnly Date { get; set; }
        public required TimeSpan Time { get; set; }
        public required int CreatedByUserId { get; set; }
        public required int MaxMembers { get; set; }
        public required int CurrentMembers { get; set; }
        public required int GroupId { get; set; }
        public string? EventPhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, EventDto>()
                .ForMember(dest => dest.MaxMembers, opt => opt.MapFrom(src => src.Group.MaxMembers))
                .ForMember(dest => dest.CurrentMembers, opt => opt.MapFrom(src => src.Group.Members.Count))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id))
                .ForMember(dest => dest.EventPhotoUrl, opt => opt.MapFrom(src => src.EventPhoto != null ? src.EventPhoto.Url : null));
        }
    }
}
