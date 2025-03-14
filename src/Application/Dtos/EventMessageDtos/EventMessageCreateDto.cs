using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventMessageDtos
{
    public class EventMessageCreateDto : IMap
    {
        public int EventId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventMessageCreateDto, EventMessage>();
        }
    }
}
