using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventDtos
{
    public class EventDto : IMap
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string SportType { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required DateTime Date { get; set; }
        public int GroupId { get; set; }
        public List<MemberDto>? Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Event, EventDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src =>
                    src.Group.Members.Select(m => new MemberDto
                    {
                        UserId = m.User.Id,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName,
                        Email = m.User.Email
                    }).ToList()
                ));
        }
    }
}
