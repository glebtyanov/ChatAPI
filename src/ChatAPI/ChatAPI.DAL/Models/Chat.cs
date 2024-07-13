namespace ChatAPI.DAL.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AdminId { get; set; }
        
        public User? Admin { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}