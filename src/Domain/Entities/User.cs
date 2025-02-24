using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : BaseDomainEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public string? Description { get; set; }
        public string? PreferredSports { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }

        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<EventAttendee> EventAttendees { get; set; }
        public ICollection<EventMessage> EventMessages { get; set; }


        [InverseProperty(nameof(Friendship.Requestor))]
        public ICollection<Friendship> FriendshipsInitiated { get; set; } = new List<Friendship>();

        [InverseProperty(nameof(Friendship.Addressee))]
        public ICollection<Friendship> FriendshipsReceived { get; set; } = new List<Friendship>();


        [InverseProperty(nameof(PrivateMessage.Sender))]
        public ICollection<PrivateMessage> SentPrivateMessages { get; set; }

        [InverseProperty(nameof(PrivateMessage.Recipient))]
        public ICollection<PrivateMessage> ReceivedPrivateMessages { get; set; }


        public ICollection<Notification> Notifications { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
