using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class ImageUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }

    public class ImageDto
    {
        public byte[] Data { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }

    public class AdvertisementImageDto
    {
        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public Guid AdvertisementId { get; set; }
    }
}
