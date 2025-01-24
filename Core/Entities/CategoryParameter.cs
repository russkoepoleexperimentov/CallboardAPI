using Common.Enums;

namespace Core.Entities
{
    public class CategoryParameter : BaseEntity
    {
        public virtual Category Category { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ParameterDataType DataType { get; set; }
        public string? EnumValues { get; set; }
    }
}
