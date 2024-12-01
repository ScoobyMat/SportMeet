namespace Domain.Entities
{
    public class Photo : BaseDomainEntity
    {
        public required string Url { get; set; }
        public string? PublicId { get; set; }
        public required int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
