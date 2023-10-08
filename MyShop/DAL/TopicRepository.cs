using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class TopicRepository : ITopicRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<TopicRepository> _logger;


    public TopicRepository(CategoryDbContext db,ILogger<TopicRepository> logger)
    {
        _db = db;
        _logger = logger;

    }

    public async Task<int?> GetRoomId(int id)
    {
        var topic = await _db.Topics.FindAsync(id);
        return topic.RoomId;
    }

    public async Task<IEnumerable<Topic>> GetAll()
    {
        try
        {
            return await _db.Topics.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicRepository] topics ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
       
    }

    public async Task<Topic?> GetItemById(int id)
    {
        try
        {
            return await _db.Topics.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicryRepository] topic FindAsync(id) failed when GetItemById for TopicId {TopicId:0000}, error message: {e}", id, e.Message);
            return null;
        }
       
    }

    public async Task<bool> Create(Topic topic)
    {
        try
        {
            _db.Topics.Add(topic);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicRepository] topic creation failed for topic {@topic}, error message: {e}", topic, e.Message);
            return false;
        }
       
    }

    public async Task<bool> Update(Topic topic)
    {
        try
        {
            _db.Topics.Update(topic);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicRepository] topic FindAsync(id) failed when updating the TopicId {TopicId:0000}, error message: {e}", topic, e.Message);
            return false;
        }
       
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var topic = await _db.Topics.FindAsync(id);
            if (topic == null)

            {
                _logger.LogError("[TopicRepository] topic not found for the TopicId {TopicId:0000}", id);
                return false;
            }
            _db.Topics.Remove(topic);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicRepository] topic deletion failed for the TopicId {TopicId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

    public async Task<List<Topic>> GetTopicByRoom(int id)
    {
        var room = await _db.Rooms.Include(r => r.Topics).FirstOrDefaultAsync(r => r.RoomId == id);
        return room?.Topics.ToList() ?? new List<Topic>();
    }
}