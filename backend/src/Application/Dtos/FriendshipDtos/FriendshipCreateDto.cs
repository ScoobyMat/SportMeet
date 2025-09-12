using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.FriendshipDtos
{
    public class FriendshipCreateDto : IMap
    {
        public int RequestorId { get; set; }
        public int AddresseeId { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<FriendshipCreateDto, Friendship>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => FriendshipStatus.Pending))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
