namespace Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string? Name { get; set; }
        public int NestedCount { get; set; }
    }
    public class CategoryFullDto : CategoryDto
    {
        public List<CategoryParameterDto> Parameters { get; set; } = null!;
    }

    public class CategoryCreateDto
    {
        public Guid? ParentId { get; set; }
        public string? Name { get; set; }
    }
}
