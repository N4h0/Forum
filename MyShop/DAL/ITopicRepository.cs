using Forum.Models;

namespace Forum.DAL
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAll();
        Task<Topic?> GetItemById(int id);
        Task<bool> Create(Topic topic);
        Task<int?> GetRoomId(int id);

        Task<bool> Update(Topic topic);
        Task<bool> Delete(int id);

        Task<List<Topic?>> GetTopicByRoom(int id);
    }
}
