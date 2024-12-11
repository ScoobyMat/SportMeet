using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.EventDtos
{
    public class EventCreateDto : IMap
    {
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string SportType { get; set; }
        public required int GroupSize { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeSpan Time { get; set; }
        public int CreatedByUserId { get; set; }
        public IFormFile? Photo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventCreateDto, Event>()
            .ForMember(dest => dest.EventPhoto, opt => opt.Ignore());
        }
    }
}
