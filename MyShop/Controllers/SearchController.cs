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

        // A Search method for handling search requests.
        // It takes a search parameter, which is provided by the user's input in the search query.
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

                //his code gets a list of categories/post/comments from the database,
                //filters them based on a case-insensitive search for search within their names, and stores the filtered list in the categories/post/comment variable.
                var categories = _db.Categories.ToList()
                     .Where(category => category.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase))
                     .ToList();
                

                var posts = _db.Posts.ToList()
                   .Where(post => post.PostTitle.Contains(search, StringComparison.OrdinalIgnoreCase))
                   .ToList();


    
                  var commment = _db.Comments.ToList()
                         .Where(commment => commment.CommentDescription.Contains(search, StringComparison.OrdinalIgnoreCase))
                             .ToList();

                    var searchResults = new SearchResultViewModel
                    {
                        Categories = categories,
                        Posts = posts,
                  
                        Comments = commment
                    };
                //Returns the list 
                    return View(searchResults);
                }
            //exception during the search operation, it logs errod and return error View 
            catch (Exception e)
                {
                    _logger.LogError(e, "An error occurred during the search.");
                    return View("Error");
                }
            }
        }
    }