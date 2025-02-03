using Core.Entities;

namespace Core.Repositiories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<List<Comment>> GetRootsForAdvertisement(Advertisement advertisement);
        Task<List<Comment>> GetNested(Comment comment);
    }
}
