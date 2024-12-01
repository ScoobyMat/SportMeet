namespace Domain.Entities
{
    public class Group : BaseDomainEntity
    {
        public required int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public List<GroupMember> Members { get; set; } = [];
    }
}
