namespace Domain.Entities
{
    public class Group : BaseDomainEntity
    {
        public required int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int MaxMembers { get; set; }
        public List<GroupMember> Members { get; set; } = [];
    }
}
