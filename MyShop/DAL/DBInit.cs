using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Forum.Models;

namespace Forum.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        CategoryDbContext context = serviceScope.ServiceProvider.GetRequiredService<CategoryDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Sport",

                },
                new Category
                {
                    Name = "Data",

                },
                new Category
                {
                    Name = "Politics",

                },
                new Category
                {
                    Name = "Health",

                },
                new Category
                {
                    Name = "Job",

                },
                new Category


                {
                    Name = "Culture,film and music",

                },
                new Category
                {
                    Name = "Mobile, tablets and smartwatches",

                },
                new Category
                {
                    Name = "Economics and law",

                },
            };
            context.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Rooms.Any())
        {
            var rooms = new List<Room>
            {
                new Room { RoomName = "Fotball", CategoryId= 1 },
                new Room { RoomName = "operatingsystem", CategoryId= 2},
            };
            context.AddRange(rooms);
            context.SaveChanges();
        }
        if (!context.Topics.Any())
        {
            var topics = new List<Topic>
            {
                new Topic { TopicName = "Liverpool", RoomId= 1 },
                new Topic { TopicName = "Linux", RoomId= 2},
            };
            context.AddRange(topics);
            context.SaveChanges();
        }
        if (!context.Posts.Any())
        {
            var posts = new List<Post>
            {
                new Post { PostTitle = "Liverpool sucks, why does it even have it's own forum?", TopicId= 1 },
                new Post { PostTitle = "\tLiverpool sucks, why does it even have it's own forum?", TopicId= 1 },
                new Post { PostTitle = "Linux is the best", TopicId= 2 },
                new Post { PostTitle = "Linux is subpar", TopicId= 2 },
                new Post { PostTitle = "Should I switch to Linux", TopicId= 2 },
                new Post { PostTitle = "Linux for gaming?", TopicId= 2 },
                new Post { PostTitle = "I switched back to windows from Linux, AMA", TopicId= 2 },
            };
            context.AddRange(posts);
            context.SaveChanges();
        }
    }
}