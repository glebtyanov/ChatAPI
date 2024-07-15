namespace ChatAPI.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ConnectionId { get; set; }
        public ICollection<Chat>? Chats { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}