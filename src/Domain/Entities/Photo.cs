namespace Domain.Entities
{
    public class Photo : BaseDomainEntity
    {
        public required string Url { get; set; }
        public string? PublicId { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? EventId { get; set; }
        public Event? Event { get; set; }
    }
}
