using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EventMessageDtos
{
    public class EventMessageDto : IMap
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventMessage, EventMessageDto>();
        }
    }
}
