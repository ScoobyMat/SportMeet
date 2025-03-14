using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.NotificationDtos
{
    public class NotificationCreateDto : IMap
    {
        public int UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotificationCreateDto, Notification>()
                   .ForMember(dest => dest.IsRead, opt => opt.Ignore())
                   .ForMember(dest => dest.Created, opt => opt.Ignore());
        }
    }
}
