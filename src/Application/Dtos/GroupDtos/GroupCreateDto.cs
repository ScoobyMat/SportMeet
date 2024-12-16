using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.GroupDtos
{
    public class GroupCreateDto : IMap
    {
        public int EventId { get; set; }
        public int CreatedByUserId { get; set; }
        public int MaxMembers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupCreateDto, Group>()
                    .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                    .ForMember(dest => dest.MaxMembers, opt => opt.MapFrom(src => src.MaxMembers));
        }
    }
}
