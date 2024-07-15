namespace ChatAPI.PL.DTO
{
    public record MessageCreateDto
    {
        public string? Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int AuthorId { get; set; }
        public int ChatId { get; set; }
    }
}