using Forum.Models;

namespace Forum.DAL
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAll();
        Task<Topic?> GetItemById(int id);
        Task<bool> Create(Topic topic);
        Task<bool> Update(Topic topic);
        Task<bool> Delete(int id);
    }
}
