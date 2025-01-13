using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.MessageDtos
{
    public class MessageDto : IMap
    {
        public int Id { get; set; }
        public required string Content { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public int SenderId { get; set; }
        public required string SenderName { get; set; } = null!;
        public int GroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.RecipientGroupId))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt));
        }
    }
}