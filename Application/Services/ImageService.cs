using Application.DTOs;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;

namespace Application.Services
{
    public class ImageService
    {
        public static string DirectoryPath = "uploaded";

        private readonly IImageRepository _imageRepository; 

        public ImageService(IImageRepository imageRepository)
        { 
            _imageRepository = imageRepository;
        }

        public async Task<ImageDto> GetAsync(Guid id)
        {
            var image = await _imageRepository.GetByIdAsync(id);

            if (image == null)
                throw new NotFoundException("Image not found");

            var data = File.ReadAllBytes(image.Path);

            return new() {
                Data = data,
                ContentType = image.ContentType!
            };
        }

        public async Task<Image> UploadAndGetAsync(User uploader, ImageUploadDto dto)
        {
            var path = "/app/images";

            if(!Directory.Exists(path)) 
                Directory.CreateDirectory(path);

            if (dto.File.ContentType.Contains("image") == false)
                throw new BadRequestException("Please, upload an image");

            var image = new Image();
            image.Uploader = uploader;
            image.ContentType = dto.File.ContentType;
            image.Path = "undefined";
            await _imageRepository.AddAsync(image);

            path = Path.Combine(path, image.Id.ToString() + "_" + dto.File.FileName);

            using (FileStream stream = File.OpenWrite(path))
            {
                await dto.File.CopyToAsync(stream);
            }

            image.Path = path;
            await _imageRepository.UpdateAsync(image);

            return image;
        }
    }
}
