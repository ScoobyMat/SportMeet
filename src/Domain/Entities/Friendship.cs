using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Friendship : BaseDomainEntity
    {

        public int RequestorId { get; set; }

        [ForeignKey(nameof(RequestorId))]
        [InverseProperty(nameof(User.FriendshipsInitiated))]
        public User Requestor { get; set; } = null!;

        public int AddresseeId { get; set; }

        [ForeignKey(nameof(AddresseeId))]
        [InverseProperty(nameof(User.FriendshipsReceived))]
        public User Addressee { get; set; } = null!;

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;

    }

    public enum FriendshipStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }
}
