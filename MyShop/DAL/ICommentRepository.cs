using Forum.Models;

namespace Forum.DAL
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<Comment?> GetItemById(int id);
        Task Create(Comment comment);
        Task Update(Comment comment);
        Task<bool> Delete(int id);
    }
}