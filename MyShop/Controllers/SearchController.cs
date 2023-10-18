using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
    public class SearchController : Controller
    {
        private readonly CategoryDbContext _context;

        public SearchController(CategoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                // Hvis søket er tomt, returner en tom liste
                return View(new SearchResultViewModel());
            }

            var categories = _context.Categories.ToList()
                .Where(category => category.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var room = _context.Rooms.ToList()
               .Where(room => room.RoomName.Contains(search, StringComparison.OrdinalIgnoreCase))
               .ToList();

            var posts = _context.Posts.ToList()
                .Where(post => post.PostTitle.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();



            var topic = _context.Topics.ToList()
                .Where(topic => topic.TopicName.Contains(search,StringComparison.OrdinalIgnoreCase)).ToList();


            var commment = _context.Comments.ToList()
                .Where(commment => commment.CommentDescription.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();


            var searchResults = new SearchResultViewModel
            {
                Categories = categories,
                Posts = posts,
                Topics= topic,
                Rooms = room,
                Comments = commment
            };

            return View(searchResults);
        }

    }
}