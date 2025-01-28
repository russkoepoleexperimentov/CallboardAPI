using Application.DTOs;
using AutoMapper;
using Common;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;
using FluentValidation;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserRegistrationDto> _registrationValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        private readonly IMapper _userMapper;
        private readonly JWTService _jwtService;
        private readonly ImageService _imageService;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IValidator<UserRegistrationDto> registrationValidator,
            IValidator<UserUpdateDto> updateValidator,
            JWTService jwtService,
            ImageService imageService)
        {
            _userRepository = userRepository;
            _registrationValidator = registrationValidator;
            _updateValidator = updateValidator;
            _userMapper = mapper;
            _jwtService = jwtService;
            _imageService = imageService;
        }

        public async Task<TokenDto> RegisterAndGetTokenAsync(UserRegistrationDto dto)
        {
            await _registrationValidator.ValidateAndThrowAsync(dto);

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new EntryExistsException($"User with Email {dto.Email} already exists.");

            var user = _userMapper.Map<UserRegistrationDto, User>(dto);

            var hashedPassword = PasswordUtils.Hashify(dto.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);

            return new() { Token = _jwtService.GenerateToken(user) };
        }

        public async Task<TokenDto> AuthenticateAndGetTokenAsync(UserAuthenticationDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                throw new InvalidAuthenticationDataException($"Invalid authentication data.");

            if(PasswordUtils.Compare(dto.Password, user.PasswordHash))
                throw new InvalidAuthenticationDataException($"Invalid authentication data.");

            return new() { Token = _jwtService.GenerateToken(user) };
        }

        public async Task<UserDto> GetMappedAsync(Guid? id)
        {
            if (id == null)
                throw new BadRequestException("Id was null");

            var user = await GetFromDbAsync(id.Value);

            var dto = _userMapper.Map<User, UserDto>(user);

            return dto;
        }

        public async Task<UserDto?> UpdateAndGetMappedAsync(Guid? id, UserUpdateDto dto)
        {
            if (id == null)
                throw new BadRequestException("Id was null");

            var user = await GetFromDbAsync(id.Value);

            await _updateValidator.ValidateAndThrowAsync(dto);

            _userMapper.Map<UserUpdateDto, User>(dto, user);

            await _userRepository.UpdateAsync(user);

            return _userMapper.Map<User, UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllMappedAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(_userMapper.Map<User, UserDto>).ToList();
        }

        public async Task<string?> UploadAvatarAsync(Guid? id, ImageUploadDto dto)
        {
            if (id == null)
                throw new BadRequestException("Id was null");

            var user = await GetFromDbAsync(id.Value);

            var image = await _imageService.UploadAndGetAsync(user, dto);

            user.AvatarId = image.Id;
            await _userRepository.UpdateAsync(user);

            return image.Path;
        }

        internal async Task<User> GetFromDbAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new NotFoundException("User not found");

            return user;
        }
    }
}
