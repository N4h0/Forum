using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

namespace Forum.DAL;

public class RoomRepository : IRoomRepository
{
    private readonly CategoryDbContext _db;

    public RoomRepository(CategoryDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Room>> GetAll()
    {
        return await _db.Rooms.ToListAsync();
    }

    public async Task<Room?> GetItemById(int id)
    {
        return await _db.Rooms.FindAsync(id);
    }

    public async Task Create(Room room)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Room room)
    {
        _db.Rooms.Update(room);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var room = await _db.Rooms.FindAsync(id);
        if (room == null)
        {
            return false;
        }

        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync();
        return true;
    }
}




