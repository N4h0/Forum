using System;
using Microsoft.EntityFrameworkCore;

namespace Forum.Models;

public class ItemDbContext : DbContext
{
    public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Category> Categories { get; set; }
    
}
