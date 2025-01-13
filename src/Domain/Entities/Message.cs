using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public required int SenderId { get; set; }
        public User Sender { get; set; } = null!;

        public required int RecipientGroupId { get; set; }
        public Group RecipientGroup { get; set; } = null!;
    }
}
