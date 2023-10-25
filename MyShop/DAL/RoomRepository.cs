using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class RoomRepository : IRoomRepository
{
    private readonly CategoryDbContext _db;
    private readonly ILogger<RoomRepository> _logger;

    public RoomRepository(CategoryDbContext db, ILogger<RoomRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Room>> GetAll()
    {
       
        try
        {
            return await _db.Rooms.ToListAsync();
        }
        catch (Exception e)
        {

            _logger.LogError("[RoomRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Room?> GetRoomById(int id)
    {
        try
        {
            return await _db.Rooms.FindAsync(id);

        }
        catch (Exception e)
        {
            _logger.LogError("Error while retrieving post with ID {id}: {e.Message}", id, e.Message);
            return null;
        }

    }

    public async Task<int?> GetCategoryId(int id)
    {
        try
        {
            var room = await _db.Rooms.FindAsync(id);
            return room.CategoryId;
        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository]  FindAsync(id) failed when GetItemById for CategoryId {CategoryId:0000}, error message: {e}", id, e.Message);
            return null;

        }
    }
    public async Task Create(Room room)
    {
        try
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository] room creation failed for room {@room}, error message: {e}", room, e.Message);
            
        }
    }

    public async Task Update(Room room)
    {
        ;
        try
        {

            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
      
        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository] room FindAsync(id) failed when updating the RoomId {TopicId:0000}, error message: {e}", room, e.Message);
       
        }
    }


    public async Task<bool> Delete(int id)
    {
        try
        {
             var room = await _db.Rooms.FindAsync(id);
              if (room == null)
              {
               return false;
             }
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return true;
        }catch(Exception e)
        {
            _logger.LogError("[RoomRepository] room deletion failed for the RoomId {RoomId:0000}, error message: {e}", id, e.Message);
            return false;
        }
     
    }
}







