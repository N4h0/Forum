using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;
using Microsoft.Extensions.Hosting;

namespace Forum.DAL;

public class PostRepository : IPostRepository
{
    private readonly CategoryDbContext _db;

    public PostRepository(CategoryDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Post>> GetAll()
    {
        return await _db.Posts.ToListAsync();
    }

    public async Task<int?> GetTopicId(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        return post.TopicId;
    }

    public async Task<Post?> GetItemById(int id)
    {
        return await _db.Posts.FindAsync(id);
    }

    public async Task Create(Post post)
    {
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Post post)
    {
        _db.Posts.Update(post);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post == null)
        {
            return false;
        }

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return true;
    }
}