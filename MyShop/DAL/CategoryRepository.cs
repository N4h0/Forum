using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryDbContext _db;

    public CategoryRepository(CategoryDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Category?> GetItemById(int id)
    {
        return await _db.Categories.FindAsync(id);
    }

    public async Task Create(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _db.Categories.FindAsync(id);
        if (item == null)
        {
            return false;
        }

        _db.Categories.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}



