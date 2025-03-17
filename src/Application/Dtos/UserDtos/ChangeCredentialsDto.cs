using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.UserDtos
{
    public class ChangeCredentialsDto : IMap
    {
        public int Id { get; set; }
        public string? NewPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChangeCredentialsDto, User>();
        }
    }
}
