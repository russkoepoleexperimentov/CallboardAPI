using Application.DTOs;
using AutoMapper;
using Common.Exceptions;
using Core.Entities;
using Core.Repositiories;
using System.Xml.Linq;

namespace Application.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly AdvertisementService _advertisementService;

        public CommentService(
            ICommentRepository repository,
            IMapper mapper,
            UserService userService,
            AdvertisementService advertisementService
            )
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _advertisementService = advertisementService;
        }

        public async Task<CommentDto> CreateRootAndGetMappedAsync(
            CommentCreateUpdateDto dto,
            Guid userId,
            Guid advertisementId)
        {
            var advertisement = await _advertisementService.GetFromDbAsync(advertisementId);
            return await CreateAndGetMappedAsync(dto, userId, advertisement, null);
        }

        public async Task<CommentDto> ReplyAndGetMappedAsync(
            CommentCreateUpdateDto dto,
            Guid userId, 
            Guid parentId)
        {
            var parent = await GetFromDbAsync(parentId);
            var advertisement = parent.Advertisement;
            return await CreateAndGetMappedAsync(dto, userId, advertisement, parent);
        }

        private async Task<CommentDto> CreateAndGetMappedAsync(
            CommentCreateUpdateDto dto,
            Guid userId,
            Advertisement advertisement,
            Comment? parent
            )
        {
            var user = await _userService.GetFromDbAsync(userId);

            var comment = _mapper.Map<CommentCreateUpdateDto, Comment>(dto);
            comment.Author = user;
            comment.Advertisement = advertisement;

            if(parent != null)
            {
                var rootId = parent.RootId ?? parent.Id;
                comment.Parent = parent;
                comment.RootId = rootId;
            }

            await _repository.AddAsync(comment);

            return _mapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<CommentDto> GetMappedAsync(Guid id)
        {
            return _mapper.Map<Comment, CommentDto>(await GetFromDbAsync(id));
        }

        public async Task<CommentDto> UpdateAndGetMappedAsync(CommentCreateUpdateDto dto, Guid id, Guid userId)
        {
            var comment = await GetFromDbAsync(id);
            var user = await _userService.GetFromDbAsync(userId);

            if (!user.IsSuperuser && comment.Author != user)
                throw new BadRequestException("No such rights for this action");

            comment.Edited = true;
            _mapper.Map<CommentCreateUpdateDto, Comment>(dto, comment);

            await _repository.UpdateAsync(comment);

            return _mapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<CommentDto> DeleteAndGetMappedAsync(Guid id, Guid userId)
        {
            var comment = await GetFromDbAsync(id);
            var user = await _userService.GetFromDbAsync(userId);

            if (!user.IsSuperuser && comment.Author != user)
                throw new BadRequestException("No such rights for this action");

            await _repository.DeleteAsync(id);

            return _mapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetMappedRootsAsync(Guid advertisementId)
        {
            var advertisement = await _advertisementService.GetFromDbAsync(advertisementId);
            var result = await _repository.GetRootsForAdvertisement(advertisement);
            return result.Select(_mapper.Map<Comment, CommentDto>).ToList();
        }

        public async Task<List<CommentDto>> GetMappedNestedAsync(Guid commentId)
        {
            var comment = await GetFromDbAsync(commentId);
            var result = await _repository.GetNested(comment);
            return result.Select(_mapper.Map<Comment, CommentDto>).ToList();
        }

        internal async Task<Comment> GetFromDbAsync(Guid id)
        {
            var comment = await _repository.GetByIdAsync(id);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            return comment;
        }
    }
}
