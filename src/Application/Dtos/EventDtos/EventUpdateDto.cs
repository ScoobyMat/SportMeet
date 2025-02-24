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
    public class EventUpdateDto : IMap
    {
        public required int Id { get; set; }

        public string? EventName { get; set; }
        public string? Description { get; set; }
        public string? SportType { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateOnly? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventUpdateDto, Event>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
