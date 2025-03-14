using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.FriendshipDtos
{
    public class FriendshipDto : IMap
    {
        public int Id { get; set; }
        public int RequestorId { get; set; }
        public int AddresseeId { get; set; }
        public string RequestorName { get; set; } = null!;
        public string RequestorNickName { get; set; } = null!;
        public string AddresseeName { get; set; } = null!;
        public string AddresseeNickName { get; set; } = null!;
        public string RequestorPhotoUrl { get; set; } = null!;
        public string AddresseePhotoUrl { get; set; } = null!;

        public string Status { get; set; } = null!;
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Friendship, FriendshipDto>()
                .ForMember(dest => dest.RequestorName, opt => opt.MapFrom(src => src.Requestor.FirstName))
                .ForMember(dest => dest.AddresseeName, opt => opt.MapFrom(src => src.Addressee.FirstName))
                .ForMember(dest => dest.RequestorNickName, opt => opt.MapFrom(src => src.Requestor.UserName))
                .ForMember(dest => dest.AddresseeNickName, opt => opt.MapFrom(src => src.Addressee.UserName))
                .ForMember(dest => dest.RequestorPhotoUrl, opt => opt.MapFrom(src => src.Requestor.PhotoUrl))
                .ForMember(dest => dest.AddresseePhotoUrl, opt => opt.MapFrom(src => src.Addressee.PhotoUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created));
        }


    }
}
