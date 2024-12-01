using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.GroupDtos
{
    public class GroupDto : IMap
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public List<GroupMemberDto> Members { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDto>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members));

        }
    }
}
