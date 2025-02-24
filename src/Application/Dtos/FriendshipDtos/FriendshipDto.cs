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
        public string AddresseeName { get; set; } = null!;

        public string Status { get; set; } = null!;
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Friendship, FriendshipDto>()
            .ForMember(d => d.RequestorName, opt => opt.MapFrom(s => s.Requestor.FirstName + " " + s.Requestor.LastName))
            .ForMember(d => d.AddresseeName, opt => opt.MapFrom(s => s.Addressee.FirstName + " " + s.Addressee.LastName))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Created, opt => opt.MapFrom(s => s.Created));
        }
    }
}
