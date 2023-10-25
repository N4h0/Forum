using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;
using Microsoft.Extensions.Hosting;

namespace Forum.DAL;

public class PostRepository : IPostRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<PostRepository> _logger;

    public PostRepository(CategoryDbContext db, ILogger<PostRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Post>> GetAll()
    {  try
        {
            return await _db.Posts.ToListAsync();
}
        catch (Exception e)
        {

            _logger.LogError("[PostRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<int?> GetTopicId(int id)
    {
        try
        {
            var post = await _db.Posts.FindAsync(id);
            return post.TopicId;
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository]  FindAsync(id) failed when GetItemById for PostId {PostId:0000}, error message: {e}", id, e.Message);
            return null;

        }

    }

    public async Task<Post?> GetPostById(int id)
    {
        try
        {  
            return await _db.Posts.FindAsync(id);

        }catch(Exception e)
        {
            _logger.LogError("Error while retrieving post with ID {id}: {e.Message}", id, e.Message);
            return null;
        }
     
    }

    public async Task Create(Post post)
    {
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        try
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[PoatRepository] post creation failed for post {@post}, error message: {e}", post, e.Message);
     
        }
    }

    public async Task Update(Post post)
    {

        try
        {

            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] podt update(id) failed when updating the PostId {PostId:0000}, error message: {e}", post, e.Message);
       
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }catch(Exception e)
        {
            _logger.LogError("[PostRepository] post deletion failed for the PostId {PostId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}


