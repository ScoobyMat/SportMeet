using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventMessageDtos
{
    public class EventMessageDto : IMap
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }

        public string SenderName { get; set; } = null!;
        public string SenderPhotoUrl { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventMessage, EventMessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.PhotoUrl));
        }
    }
}
