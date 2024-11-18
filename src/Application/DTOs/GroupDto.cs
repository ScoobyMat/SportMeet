using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GroupDto : IMap
    {
        public int Id { get; set; }
        public int ManagedByUserId { get; set; }
        public List<AppUser>? Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupDto>();
        }
    }
}
