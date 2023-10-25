using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(CategoryDbContext db, ILogger<CategoryRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>?> GetAll()
    {
        try
        {
            return await _db.Categories.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        _logger.LogInformation("[CategoryRep] Getting the category with id: {id}: ", id);
        try
        {
            return await _db.Categories.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category FindAsync(id) failed when GetItemById for CategoryId {CategoryId}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Category category)
    {
        try
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category creation failed for item {@category}, error message: {e}", category, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Category category)
    {
        try
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category FindAsync(id) failed when updating the CategoryId {CategoryId:0000}, error message: {e}", category, e.Message);
            return false;
        }

    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.Categories.FindAsync(id);
            if (item == null)
            {
                _logger.LogError("[CategoryRepository] category not found for the CategoryId {CategoryId:0000}", id);
                return false;
            }

            _db.Categories.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[CategoryRepository] category deletion failed for the CategoryId {CategoryId:0000}, error message: {e}", id, e.Message);
            return false;
        }

    }
}



