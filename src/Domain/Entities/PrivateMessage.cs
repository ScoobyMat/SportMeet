using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class PrivateMessage : BaseDomainEntity
    {
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.SentPrivateMessages))]
        public User Sender { get; set; }

        public int RecipientId { get; set; }
        [ForeignKey(nameof(RecipientId))]
        [InverseProperty(nameof(User.ReceivedPrivateMessages))]
        public User Recipient { get; set; }

        public string Content { get; set; }
    }
}
