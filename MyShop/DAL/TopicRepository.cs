using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class TopicRepository : ITopicRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<TopicRepository> _logger;


    // Constructor for the TopicRepository class that takes two parameters: CategoryDbContext and ILogger.

    public TopicRepository(CategoryDbContext db,ILogger<TopicRepository> logger)
    {
        _db = db;
        _logger = logger;

    }

    // Define a method to get the RoomId associated with a topic by its ID.


    public async Task<int?> GetRoomId(int id)
    {
        // Try to find and get the topic based on its ID.
        var topic = await _db.Topics.FindAsync(id);

        // Return the RoomId associated with the topic.
        return topic.RoomId;
    }


    // Method to get all topics asynchronously.

    public async Task<IEnumerable<Topic>> GetAll()
    {
        // Try to fetch all topics from the database and return them.

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
    // Method to get a topic by its ID.

    public async Task<Topic?> GetTopicById(int id)
    {

        // Try to find and return a topic based on its ID.

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


    //  method to create a new topic.

    public async Task<bool> Create(Topic topic)
    {
        try
        {
            // Add the new topic to the database and then Save changes to the database asynchronously.

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

    //  method to update an existing topic.

    public async Task<bool> Update(Topic topic)
    {
        try
        {
            // Update the topic in the database context and then  Save changes to the database asynchronously.
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

    // Define a method to delete a topic by its ID.


    public async Task<bool> Delete(int id)
    {
        // Try to find the topic by its ID.

        try
        {

            var topic = await _db.Topics.FindAsync(id);
            if (topic == null)

            {
                _logger.LogError("[TopicRepository] topic not found for the TopicId {TopicId:0000}", id);
                return false;
            }
            // Remove the topic from the database

            _db.Topics.Remove(topic);
            // Remove the topic from the database 

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[TopicRepository] topic deletion failed for the TopicId {TopicId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

    // method to get a list of topics associated with a room by its ID.


    public async Task<List<Topic>> GetTopicByRoom(int id)
    {
        // Try to retrieve a room and include its related topics from the database, 

        var room = await _db.Rooms.Include(r => r.Topics).FirstOrDefaultAsync(r => r.RoomId == id);
        // If the room exists, return
        return room?.Topics.ToList() ?? new List<Topic>();
    }
}