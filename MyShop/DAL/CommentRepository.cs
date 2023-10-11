using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class CommentRepository : ICommentRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<CommentRepository> _logger;
    public CommentRepository(CategoryDbContext db, ILogger<CommentRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<int?> GetPostId(int id)
    {
        var comment = await _db.Comments.FindAsync(id);
        return comment.PostId;
    }

    public async Task<IEnumerable<Comment>> GetAll()
    {
        try
        {  
            return await _db.Comments.ToListAsync();

        }catch (Exception e)
        {
            _logger.LogError("[CommentRepository] comment ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
      
    }

    public async Task<Comment?> GetItemById(int id)
    {
        try
        {
         return await _db.Comments.FindAsync(id);
        }catch (Exception e)
        {
            _logger.LogError("[CommentRepository] Comment FindAsync(id) failed when GetItemById " +
                "for CommentId {CommentId:0000}, error message: [e} ", id, e.Message);
            return null;
        }
        
    }

    public async Task Create(Comment Comment)
    {
        try
        {
            _db.Comments.Add(Comment);
            await _db.SaveChangesAsync();
            
        }catch(Exception e)
        {
            _logger.LogError("[CommentRepository] comment creation failed for CommentID {@comment}, error message: {e}",Comment, e.Message);

        }

    }

    public async Task Update(Comment Comment)
    {
        _db.Comments.Update(Comment);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var Comment = await _db.Comments.FindAsync(id);
            if (Comment == null)
            {
                return false;
            }
            _db.Comments.Remove(Comment);
            await _db.SaveChangesAsync();
            return true;
        }catch (Exception e) {

            _logger.LogError("[CommenRepository] commen deletion failed for the CommentId {CommentId:0000}, error message: {e}", id, e.Message);
            return false;

        }

    }
}