namespace Domain.Entities
{
    public class GroupMember : BaseDomainEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Role { get; set; }
    }
}
