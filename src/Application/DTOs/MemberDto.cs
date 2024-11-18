using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs.User
{
    public class MemberDto : IMap
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, MemberDto>();
        }
    }
}
