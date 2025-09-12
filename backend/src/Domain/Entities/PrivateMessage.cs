using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }


        [ForeignKey(nameof(RecipientId))]
        [InverseProperty(nameof(User.ReceivedPrivateMessages))]
        public User Recipient { get; set; }


        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.SentPrivateMessages))]
        public User Sender { get; set; }
    }
}
