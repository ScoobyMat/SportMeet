using System;
using Application.DTOs.User;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.Auth;

public class AuthResponseDto : IMap
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Token { get; set; }
    public string? PhotoUrl { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProfilePhoto == null ? null : src.ProfilePhoto.Url));
    }
}
