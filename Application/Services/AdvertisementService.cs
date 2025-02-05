using Application.DTOs;
using Application.Validators;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;
using FluentValidation;
using System.Collections.Generic;

namespace Application.Services
{
    public class AdvertisementService
    {
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        private readonly CategoryParameterService _categoryParameterService;
        private readonly AdvertisementParameterValueService _advertismentParameterValueService;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;
        private readonly IAdvertisementImageRepository _advertisementImageRepository;
        private readonly ImageService _imageService;

        private readonly IValidator<AdvertisementCreateDto> _createValidator;
        private readonly IValidator<AdvertisementUpdateDto> _updateValidator;

        public AdvertisementService(
            IAdvertisementRepository advertisementRepository,
            UserService userService,
            CategoryService categoryService,
            CategoryParameterService categoryParameterService,
            AdvertisementParameterValueService advertismentParameterValueService,
            IMapper mapper,
            IAdvertisementImageRepository advertisementImageRepository,
            ImageService imageService,
            IValidator<AdvertisementCreateDto> createValidator,
            IValidator<AdvertisementUpdateDto> updateValidator
            )
        {
            _advertisementRepository = advertisementRepository;
            _userService = userService;
            _categoryService = categoryService;
            _categoryParameterService = categoryParameterService;
            _advertismentParameterValueService = advertismentParameterValueService;
            _mapper = mapper;
            _advertisementImageRepository = advertisementImageRepository;
            _imageService = imageService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<List<AdvertisementDto>> SearchMappedAsync(AdvertisementSearchDto dto)
        {
            var categoriesIds = dto.CategoryId != null ?
                (await _categoryService.GetNestedFromDbAsync(dto.CategoryId.Value, true)).Select(x => x.Id).ToList() :
                null;

            if(categoriesIds != null && categoriesIds.Count == 1)
            {
                if (dto.ParameterEqualsCriteria.Any() || dto.ParameterRangeCriteria.Any())
                {
                    var parametersWithRangeIds = dto.ParameterRangeCriteria.Select(x => x.Key).ToList();
                    var parametersWithEqualtyIds = dto.ParameterEqualsCriteria.Select(x => x.Key).ToList();
                    var parametersIds = new List<Guid>();
                    parametersIds.AddRange(parametersWithRangeIds);
                    parametersIds.AddRange(parametersWithEqualtyIds);
                    var category = await _categoryService.GetFromDbAsync(categoriesIds.First());

                    foreach (var parameterId in parametersIds)
                    {
                        var parameter = await _categoryParameterService.GetParameterFromDbAsync(parameterId);

                        if (parameter.Category != category)
                            throw new BadRequestException($"Parameter {parameterId} is not a part of the category {category.Id}");
                        
                        if(parametersWithRangeIds.Contains(parameterId))
                        {
                            if(parameter.DataType != ParameterDataType.Integer &&
                                parameter.DataType != ParameterDataType.Float)
                                throw new BadRequestException($"Parameter {parameterId} with type {parameter.DataType} can't be used in range criteria");
                        }
                    }
                }
            }
            else
            {
                if (dto.ParameterEqualsCriteria.Any() || dto.ParameterRangeCriteria.Any())
                    throw new BadRequestException("Criterias can only be used with category without children");
            }

            var result = await _advertisementRepository.Search(dto.Query, categoriesIds, dto.Sorting, 
                dto.ParameterEqualsCriteria, dto.ParameterRangeCriteria.ToDictionary(x => x.Key, x => (x.Value.Min, x.Value.Max)), dto.Skip, dto.Take);
            List<AdvertisementDto> dtos = new();

            foreach (var item in result) 
                dtos.Add(await GetMappedAsync(item));

            return dtos;
        }

        public async Task<AdvertisementImageDto> AddImageAsync(
            Guid advertisementId,
            Guid userId,
            ImageUploadDto dto
            )
        {
            var advertisement = await GetFromDbAsync(advertisementId);
            var user = await _userService.GetFromDbAsync(userId);

            if (advertisement.Author != user && user.IsSuperuser == false)
                throw new BadRequestException("Not allowed");

            var image = await _imageService.UploadAndGetAsync(user, dto);
            var advertisementImage = new AdvertisementImage();
            advertisementImage.Advertisement = advertisement;
            advertisementImage.Image = image;

            await _advertisementImageRepository.AddAsync(advertisementImage);

            return _mapper.Map<AdvertisementImage, AdvertisementImageDto>(advertisementImage);
        }

        public async Task<AdvertisementImageDto> RemoveImageAsync(
            Guid advertisementId,
            Guid userId,
            Guid advertisementImageId
            )
        {
            var advertisement = await GetFromDbAsync(advertisementId);
            var advertisementImage = await _advertisementImageRepository.GetByIdAsync(advertisementImageId);

            if (advertisementImage == null)
                throw new NotFoundException("AdvertisementImage not found");

            var user = await _userService.GetFromDbAsync(userId);

            if (advertisement.Author != user && user.IsSuperuser == false)
                throw new BadRequestException("Not allowed");

            if(advertisementImage.Advertisement != advertisement)
                throw new BadRequestException("That image is not part of this advertisement");

            await _advertisementImageRepository.DeleteAsync(advertisementImage.Id);

            return _mapper.Map<AdvertisementImage, AdvertisementImageDto>(advertisementImage);
        }

        public async Task<AdvertisementDto> GetMappedAsync(Guid id)
        {
            return await GetMappedAsync(await GetFromDbAsync(id));
        }

        public async Task<AdvertisementDto> CreateAndGetMappedAsync(
            Guid userId, 
            AdvertisementCreateDto dto
            )
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var user = await _userService.GetFromDbAsync(userId);
            var category = await _categoryService.GetFromDbAsync(dto.CategoryId);
            var parameters = await _categoryParameterService.GetParametersForCategoryFromDbAsync(category);
            var parametersCheck = parameters.Select(x => x.Id).ToDictionary(x => x, x => false);

            if (parameters.Count != dto.Parameters.Count)
                throw new BadRequestException("Parameters count mismatch");

            foreach (var parameter in dto.Parameters)
            {
                if(!parametersCheck.ContainsKey(parameter.ParameterId))
                    throw new BadRequestException($"Invalid parameter {parameter.ParameterId}");

                if (parametersCheck[parameter.ParameterId] == true)
                    throw new BadRequestException($"Parameter duplication {parameter.ParameterId}");
            }

            var parameterValues = await _advertismentParameterValueService.CreateAndGetListAsync(dto.Parameters);

            var advertisement = _mapper.Map<AdvertisementCreateDto, Advertisement>(dto);
            advertisement.Author = user;
            advertisement.Category = category;

            await _advertisementRepository.AddAsync(advertisement);
            await _advertismentParameterValueService.AddListToDbAsync(advertisement, parameterValues);

            return await GetMappedAsync(advertisement);
        }

        public async Task<AdvertisementDto> DeleteAndGetMappedAsync(
            Guid id, 
            Guid userId
            )
        {
            var user = await _userService.GetFromDbAsync(userId);
            var advertisment = await GetFromDbAsync(id);

            if(!user.IsSuperuser && advertisment.Author != user) 
                throw new BadRequestException("You can't do this");

            await _advertisementRepository.DeleteAsync(id);

            return await GetMappedAsync(advertisment);
        }

        public async Task<AdvertisementDto> UpdateAndGetMappedAsync(
            Guid id, 
            Guid userId,
            AdvertisementUpdateDto dto
            )
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var user = await _userService.GetFromDbAsync(userId);
            var advertisment = await GetFromDbAsync(id);

            if(!user.IsSuperuser && advertisment.Author != user) 
                throw new BadRequestException("You can't do this");

            _mapper.Map(dto, advertisment);

            await _advertisementRepository.UpdateAsync(advertisment);

            return await GetMappedAsync(advertisment);
        }

        internal async Task<Advertisement> GetFromDbAsync(Guid id)
        {
            var item = await _advertisementRepository.GetByIdAsync(id);

            if(item == null)
                throw new NotFoundException("Advertisment not found");

            return item;
        }

        private async Task<AdvertisementDto> GetMappedAsync(Advertisement advertisment)
        {
            var result = _mapper.Map<Advertisement, AdvertisementDto>(advertisment);
            result.Category = await _categoryService.GetFullMappedFromAsync(advertisment.Category);
            result.Parameters = await _advertismentParameterValueService.GetAllForAdvertisementAsync(advertisment);
            result.Images = (await _advertisementImageRepository.GetForAdvertisement(advertisment))
                .Select(_mapper.Map<AdvertisementImage, AdvertisementImageDto>).ToList();
            return result;
        }
    }
}
