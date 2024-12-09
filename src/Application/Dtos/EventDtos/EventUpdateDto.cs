using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.EventDtos
{
    public class EventUpdateDto : IMap
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public string? SportType { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateOnly? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventUpdateDto, Event>();
        }
    }
}
