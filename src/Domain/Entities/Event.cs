namespace Domain.Entities
{
    public class Event : BaseDomainEntity
    {
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string SportType { get; set; }
        public required int MaxParticipants { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeSpan Time { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public Group Group { get; set; } = null!;
    }
}
