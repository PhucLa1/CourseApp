namespace Dtos.Models.EmailModels
{
    public record Email
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
