using System;
using Microsoft.EntityFrameworkCore;

namespace Forum.Models;

public class RoomDbContext : DbContext
{
    public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Room> Rooms { get; set; }
}
