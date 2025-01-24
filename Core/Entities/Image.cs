using System.ComponentModel;

namespace Core.Entities
{
    public class Image : BaseEntity
    {
        public virtual User Uploader { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string? ContentType { get; set; }
    }
}
