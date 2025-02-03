using Core.Entities;
using Core.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Comment>> GetNested(Comment comment)
        {
            return await Set.Where(x => x.RootId == comment.Id).ToListAsync();
        }

        public async Task<List<Comment>> GetRootsForAdvertisement(Advertisement advertisement)
        {
            return await Set.Where(x => x.RootId == null && x.Advertisement == advertisement).ToListAsync();
        }
    }
}
