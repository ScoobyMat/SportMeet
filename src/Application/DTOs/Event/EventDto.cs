using Application.DTOs.User;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class EventDto : IMap
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public required string Location { get; set; }
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
        public required List<MemberDto> Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Event, EventDto>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.Group.Id))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Group.Members));
        }

    }
}
