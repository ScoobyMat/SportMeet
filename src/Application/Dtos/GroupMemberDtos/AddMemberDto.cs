namespace Application.Dtos.GroupMemberDtos
{
    public class AddMemberDto
    {
        public int GroupId {  get; set; } 
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
