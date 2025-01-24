using Common.Enums;

namespace Application.DTOs
{
    public class CategoryParameterDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public List<string>? EnumValues { get; set; }
        public ParameterDataType DataType { get; set; }
    }

    public class CategoryParameterCreateDto
    {
        public string Name { get; set; } = null!;
        public ParameterDataType DataType { get; set; }
        public List<string>? EnumValues { get; set; }
    }

    public class CategoryParameterUpdateDto
    {
        public string Name { get; set; } = null!;
    }
}
