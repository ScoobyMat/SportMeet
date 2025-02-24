namespace Domain.Entities
{
    public class EventMessage : BaseDomainEntity
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public string Content { get; set; }

    }
}
