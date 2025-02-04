namespace Core.Entities
{
    public class AdvertisementImage : BaseEntity
    {
        public virtual Advertisement Advertisement { get; set; } = null!;
        public virtual Image Image { get; set; } = null!;
    }
}
