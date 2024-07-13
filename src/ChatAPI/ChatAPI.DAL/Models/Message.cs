namespace ChatAPI.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int AuthorId { get; set; }
        public int ChatId { get; set; }
        
        public User? Author { get; set; }
        public Chat? Chat { get; set; }
    }
}