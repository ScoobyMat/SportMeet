using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.NotificationDtos
{
    public class NotificationDto : IMap
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Notification, NotificationDto>();
        }
    }
}
