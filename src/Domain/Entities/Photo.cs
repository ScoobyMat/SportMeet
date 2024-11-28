namespace Domain.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public string? PublicId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
