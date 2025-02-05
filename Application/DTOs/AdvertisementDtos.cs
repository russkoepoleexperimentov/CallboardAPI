
using Common.Enums;
using System.Text.Json;

namespace Application.DTOs
{
    public class AdvertisementDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Cost { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public UserDto Author { get; set; } = null!;
        public List<AdvertisementParameterValueDto> Parameters { get; set; } = null!;
        public List<AdvertisementImageDto> Images { get; set; } = null!;
    }
    public class AdvertisementSearchDto
    {
        public string? Query { get; set; } = null;
        public Guid? CategoryId { get; set; } = null; 
        public Dictionary<Guid, List<JsonElement>> ParameterEqualsCriteria { get; set; } = new();
        public Dictionary<Guid, SearchRangeCriteria> ParameterRangeCriteria { get; set; } = new();
        public AdvertisementSorting Sorting { get; set; } = AdvertisementSorting.DateAsc;
        public SearchIntRangeCriteria? CostRange { get; set; }
        public bool OnlyWithImages { get; set; } = false;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
    }

    public class SearchRangeCriteria
    {
        public JsonElement Min { get; set; }
        public JsonElement Max { get; set; }
    }

    public class SearchIntRangeCriteria
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class AdvertisementCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
        public Guid CategoryId { get; set; }
        public List<AdvertisementParameterValueCreateDto> Parameters { get; set; } = null!;
    }

    public class AdvertisementUpdateDto
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public int? Cost { get; set; } = null;
    }
}