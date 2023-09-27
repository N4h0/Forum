using Forum.Models;
namespace Forum.DAL
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category?> GetItemById(int id);
        Task Create(Category category);
        Task Update(Category category);
        Task<bool> Delete(int id);
    }
}
