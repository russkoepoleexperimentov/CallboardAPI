
using Common.Enums;
using System.Text.Json;

namespace Application.DTOs
{
    public class AdvertismentParameterValueDto
    {
        public Guid Id { get; set; }
        public Guid ParameterId {  get; set; }
        public string Name { get; set; }
        public ParameterDataType DataType { get; set; }
        public object Value { get; set; }
    }
    public class AdvertismentParameterValueCreateDto
    {
        public Guid ParameterId { get; set; }
        public JsonElement Value { get; set; }
    }
}
