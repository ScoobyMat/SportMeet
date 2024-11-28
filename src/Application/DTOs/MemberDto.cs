using AutoMapper;
using Domain.Entities;
using Application.Dtos.UserDtos;
using Application.Mappings;

public class MemberDto : IMap
{
    public required int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
    }
}
