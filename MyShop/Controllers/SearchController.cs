using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
    public class SearchController : Controller
    {
        private readonly CategoryDbContext _db;
        private readonly ILogger<SearchController> _logger; 

        public SearchController(CategoryDbContext db, ILogger<SearchController> logger) 
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
   
            [HttpGet]
            public IActionResult Search(string search)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(search))
                    {
                   // If the query is empty, return an empty list
                    return View(new SearchResultViewModel());
                    }


                    var categories = _db.Categories.ToList()
                     .Where(category => category.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                     .ToList();
                   var room = _db.Rooms.ToList()
                     .Where(room => room.RoomName.Contains(search, StringComparison.OrdinalIgnoreCase))
                     .ToList();

                var posts = _db.Posts.ToList()
                   .Where(post => post.PostTitle.Contains(search, StringComparison.OrdinalIgnoreCase))
                   .ToList();


    
                 var topic = _db.Topics.ToList()
                     .Where(topic => topic.TopicName.Contains(search,StringComparison.OrdinalIgnoreCase)).ToList();


                  var commment = _db.Comments.ToList()
                         .Where(commment => commment.CommentDescription.Contains(search, StringComparison.OrdinalIgnoreCase))
                             .ToList();

                    var searchResults = new SearchResultViewModel
                    {
                        Categories = categories,
                        Posts = posts,
                        Topics = topic,
                        Rooms = room,
                        Comments = commment
                    };

                    return View(searchResults);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occurred during the search."); // Legg til logging for feil
                    return View("Error");
                }
            }
        }
    }