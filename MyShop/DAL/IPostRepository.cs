using Forum.Models;

namespace Forum.DAL
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAll();
        Task<Post?> GetPostById(int id);
        Task<int?> GetTopicId(int id);
        Task Create(Post post);
        Task Update(Post post);
        Task<bool> Delete(int id);
    }
}
