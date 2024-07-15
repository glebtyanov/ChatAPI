namespace ChatAPI.PL.DTO
{
    public record ChatCreateDto
    {
        public string? Name { get; set; }
        public int AdminId { get; set; }
    }
}