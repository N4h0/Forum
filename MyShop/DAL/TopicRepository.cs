using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class TopicRepository : ITopicRepository
{
    private readonly CategoryDbContext _db;

    public TopicRepository(CategoryDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Topic>> GetAll()
    {
        return await _db.Topics.ToListAsync();
    }

    public async Task<Topic?> GetItemById(int id)
    {
        return await _db.Topics.FindAsync(id);
    }

    public async Task Create(Topic topic)
    {
        _db.Topics.Add(topic);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Topic topic)
    {
        _db.Topics.Update(topic);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var topic = await _db.Topics.FindAsync(id);
        if (topic == null)
        {
            return false;
        }

        _db.Topics.Remove(topic);
        await _db.SaveChangesAsync();
        return true;
    }
}





