using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class CommentRepository : ICommentRepository
{
    private readonly CategoryDbContext _db;
    public CommentRepository(CategoryDbContext db)
    {
        _db = db;
    }

    public async Task<int?> GetPostId(int id)
    {
        var comment = await _db.Comments.FindAsync(id);
        return comment.PostId;
    }

    public async Task<IEnumerable<Comment>> GetAll()
    {
        return await _db.Comments.ToListAsync();
    }

    public async Task<Comment?> GetItemById(int id)
    {
        return await _db.Comments.FindAsync(id);
    }

    public async Task Create(Comment Comment)
    {
        _db.Comments.Add(Comment);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Comment Comment)
    {
        _db.Comments.Update(Comment);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var Comment = await _db.Comments.FindAsync(id);
        if (Comment == null)
        {
            return false;
        }
        _db.Comments.Remove(Comment);
        await _db.SaveChangesAsync();
        return true;
    }
}