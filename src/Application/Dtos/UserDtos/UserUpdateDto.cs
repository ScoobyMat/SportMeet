using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.UserDtos
{
    public class UserUpdateDto : IMap
    {
        public required int Id { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? PreferredSports { get; set; }
        public string? PhotoUrl { get; set; }
        public IFormFile? Photo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
