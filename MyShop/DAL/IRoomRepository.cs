using Forum.Models;

namespace Forum.DAL
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAll();
        Task<Room?> GetItemById(int id);
        Task<int?> GetCategoryId(int id);
        Task Create(Room room);
        Task Update(Room room);
        Task<bool> Delete(int id);
    }
}
