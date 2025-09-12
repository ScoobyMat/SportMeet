using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.PrivateMessageDtos
{
    public class PrivateMessageDto : IMap
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }
        public string SenderName { get; set; } = null!;
        public string SenderPhotoUrl { get; set; } = null!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateMessage, PrivateMessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.PhotoUrl));
        }
    }
}
