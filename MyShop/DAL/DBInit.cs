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

        if (2 == 2) {
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
                new Post { PostTitle = "I love Liverpool, it's the best team ever", TopicId= 1 },
                new Post { PostTitle = "Linux is the best", TopicId= 2 },
                new Post { PostTitle = "Linux is subpar", TopicId= 2 },
                new Post { PostTitle = "Should I switch to Linux", TopicId= 2 },
                new Post { PostTitle = "Linux for gaming?", TopicId= 2 },
                new Post { PostTitle = "I switched back to windows from Linux, AMA", TopicId= 2 },
            };
            context.AddRange(posts);
            context.SaveChanges();
        }
        if (!context.Comments.Any())
        {
            var comments = new List<Comment>
            {
                new Comment { CommentDescription = "See title!", PostId= 1 },
                new Comment { CommentDescription = "Go away!", PostId= 1 },
                new Comment { CommentDescription = "You are so mean!", PostId= 1 },
                new Comment { CommentDescription = "Also, please ignore the hater in the last post", PostId= 2 },
                new Comment { CommentDescription = "Liiinux is love Linux is liiiife!!!!", PostId= 3 },
                new Comment { CommentDescription = "Linux is bad", PostId= 4 },
                new Comment { CommentDescription = "No", PostId= 5 },
                new Comment { CommentDescription = "Yes, it's 2023", PostId= 6 },
                new Comment { CommentDescription = "Shut up", PostId= 7 },

            };
            context.AddRange(comments);
            context.SaveChanges();
        }
    }
    }
}