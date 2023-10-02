using Forum.Models;
namespace Forum.DAL
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category?> GetItemById(int id);
        Task<bool> Create(Category category);
        Task<bool> Update(Category category);
        Task<bool> Delete(int id);
    }
}
