using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.GroupDtos
{
    public class GroupUpdateDto : IMap
    {
        public int GroupId { get; set; }
        public int CurrentManagerId { get; set; }
        public int NewManagerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupUpdateDto, Group>();
        }
    }
}
