
namespace Application.DTOs
{
    public class AdvertisementDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Cost { get; set; }
        public int? OldCost { get; set; }
        public CategoryFullDto Category { get; set; } = null!;
        public UserDto Author { get; set; } = null!;
        public List<AdvertisementParameterValueDto> Parameters { get; set; } = null!;
    }

    public class AdvertisementCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
        public Guid CategoryId { get; set; }
        public List<AdvertisementParameterValueCreateDto> Parameters { get; set; } = null!;
    }
}
