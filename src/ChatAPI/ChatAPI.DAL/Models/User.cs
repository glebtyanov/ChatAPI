namespace ChatAPI.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        
        public ICollection<Chat>? Chats { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}