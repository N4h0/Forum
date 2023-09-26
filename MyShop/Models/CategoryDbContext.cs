using System;
using Microsoft.EntityFrameworkCore;

namespace Forum.Models;

public class CategoryDbContext : DbContext
{
    public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Thread> Threads { get; set; }

}
