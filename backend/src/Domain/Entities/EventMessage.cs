namespace Domain.Entities
{
    public class EventMessage
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public string Content { get; set; }
        public DateTime Created { get; set; }

    }
}
