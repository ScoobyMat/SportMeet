using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.MessageDtos
{
    public class CreateMessageDto : IMap
    {
        public required string Content { get; set; } = null!;
        public int SenderId { get; set; }
        public int GroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMessageDto, Message>()
                .ForMember(dest => dest.RecipientGroupId, opt => opt.MapFrom(src => src.GroupId));
        }

    }
}
