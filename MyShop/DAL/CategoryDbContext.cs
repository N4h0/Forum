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
    public DbSet<Topic> Topics{ get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        object value = optionsBuilder.UseLazyLoadingProxies();
    }

}
