using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.EventDtos
{
    public class EventCreateDto : IMap
    {
        [Required]
        public string EventName { get; set; }
        public string? Description { get; set; }
        [Required]
        public string SportType { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeSpan Time { get; set; }
        public IFormFile? Photo { get; set; }
        [Required]
        public int CreatedByUserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventCreateDto, Event>();
        }
    }
}
