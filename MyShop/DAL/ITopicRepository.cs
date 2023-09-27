using Forum.Models;

namespace Forum.DAL
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAll();
        Task<Topic?> GetItemById(int id);
        Task Create(Topic topic);
        Task Update(Topic topic);
        Task<bool> Delete(int id);
    }
}
