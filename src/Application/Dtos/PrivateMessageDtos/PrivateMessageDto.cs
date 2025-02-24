namespace Application.Dtos.PrivateMessageDtos
{
    public class PrivateMessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
