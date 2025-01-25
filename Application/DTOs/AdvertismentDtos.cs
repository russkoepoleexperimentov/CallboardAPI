
namespace Application.DTOs
{
    public class AdvertismentDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Cost { get; set; }
        public int? OldCost { get; set; }
        public CategoryFullDto Category { get; set; } = null!;
        public UserDto Author { get; set; } = null!;
        public List<AdvertismentParameterValueDto> Parameters { get; set; } = null!;
    }

    public class AdvertismentCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
        public Guid CategoryId { get; set; }
        public List<AdvertismentParameterValueCreateDto> Parameters { get; set; } = null!;
    }
}
