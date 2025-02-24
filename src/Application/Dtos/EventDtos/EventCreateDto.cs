using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EventDtos
{
    public class EventCreateDto : IMap
    {
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string SportType { get; set; }
        public required int MaxParticipants { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public required DateOnly Date { get; set; }
        public required TimeSpan Time { get; set; }

        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }

        public required int CreatedByUserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventCreateDto, Event>();
        }
    }
}
