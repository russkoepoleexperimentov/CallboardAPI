
using Common.Enums;
using System.Text.Json;

namespace Application.DTOs
{
    public class AdvertisementParameterValueDto
    {
        public Guid Id { get; set; }
        public Guid ParameterId {  get; set; }
        public string Name { get; set; } = null!;
        public ParameterDataType DataType { get; set; }
        public object Value { get; set; } = null!;
    }
    public class AdvertisementParameterValueCreateDto
    {
        public Guid ParameterId { get; set; }
        public JsonElement Value { get; set; }
    }
}
