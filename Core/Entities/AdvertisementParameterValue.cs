namespace Core.Entities
{
    public class AdvertisementParameterValue : BaseEntity
    {
        public virtual Advertisement Advertisment { get; set; } = null!;
        public virtual CategoryParameter CategoryParameter { get; set; } = null!;
        public int? IntegerValue { get; set; }
        public float? FloatValue { get; set; }
        public bool? BooleanValue { get; set; }
        public string? StringValue { get; set; }
        public int? EnumValue { get; set; }
    }
}
