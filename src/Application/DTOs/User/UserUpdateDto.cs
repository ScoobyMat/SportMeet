using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class UserUpdateDto : IMap
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserUpdateDto, AppUser>()
                 .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.PhotoUrl != null ? new Photo { Url = src.PhotoUrl } : null));
        }
    }
}
