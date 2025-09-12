using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Dtos.PrivateMessageDtos
{
    public class PrivateMessageCreateDto : IMap
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateMessageCreateDto, PrivateMessage>();
        }
    }
}
