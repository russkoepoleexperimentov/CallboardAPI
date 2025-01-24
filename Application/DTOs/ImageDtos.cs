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
}
