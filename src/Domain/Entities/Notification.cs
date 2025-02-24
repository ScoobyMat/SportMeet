namespace Domain.Entities
{
    public class Notification : BaseDomainEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string Type { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }
    }
}
