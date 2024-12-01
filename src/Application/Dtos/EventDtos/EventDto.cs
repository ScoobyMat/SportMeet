using Application.Dtos.GroupDtos;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;

public class EventDto : IMap
{
    public int Id { get; set; }
    public required string EventName { get; set; }
    public string? Description { get; set; }
    public required string SportType { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required DateTime Date { get; set; }
    public int CreatedByUserId { get; set; }
    public GroupDto Group { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Event, EventDto>();
    }
}
