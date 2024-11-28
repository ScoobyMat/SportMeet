using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.EventDtos
{
    public class EventCreateDto : IMap
    {
        public required string EventName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Opis wydarzenia nie może przekraczać 100 znaków.")]
        public string Description { get; set; } = string.Empty;

        public required string SportType { get; set; } = string.Empty;
        public required string Address { get; set; } = string.Empty;
        public required string City { get; set; } = string.Empty;
        public required DateTime Date { get; set; }
        public int CreatedByUserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventCreateDto, Event>();
        }
    }
}
