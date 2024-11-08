using System;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs;

public class UserDto : IMap
{
    public required string FirstName { get; set; }
    public required string LastName { get; set;}
    public required string Token { get; set; }
    public string? PhotoUrl { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProfilePhoto == null ? null : src.ProfilePhoto.Url));
    }
}
