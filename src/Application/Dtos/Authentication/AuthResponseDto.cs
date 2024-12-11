using Application.Dtos.UserDtos;
using AutoMapper;
using Application.Mappings;
using Domain.Entities;

namespace Application.Dtos.Authentication
{
    public class AuthResponseDto : IMap
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProfilePhoto == null ? null : src.ProfilePhoto.Url));
        }
    }
}