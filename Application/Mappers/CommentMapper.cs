using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers
{
    public class CommentMapper : Profile
    {
        public CommentMapper() 
        {
            CreateMap<Comment, CommentDto>();

            CreateMap<CommentCreateUpdateDto, Comment>();
        }
    }
}
