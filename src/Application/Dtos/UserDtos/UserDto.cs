using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.UserDtos;

public class UserDto : IMap
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int Age { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? PhotoUrl { get; set; }
    public required string Gender { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public string? Description { get; set; }
    public string? PreferredSports { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>();
    }
}