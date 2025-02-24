namespace Application.Dtos.EventMessageDtos
{
    public class EventMessageCreateDto
    {
        public int EventId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
    }
}
