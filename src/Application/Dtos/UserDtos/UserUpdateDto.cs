using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.UserDtos
{
    public class UserUpdateDto : IMap
    {
        public required int Id { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? PreferredSports { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.ProfilePhoto, opt =>
                    opt.MapFrom(src =>
                        src.PhotoUrl != null ? new Photo { Url = src.PhotoUrl } : null
                    ));
        }

    }
}
