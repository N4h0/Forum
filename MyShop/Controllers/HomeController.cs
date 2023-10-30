using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.DAL; // Importer riktig namespace for PostRepository
using Forum.ViewModels; // Importer riktig namespace for PostListViewModel
using Microsoft.AspNetCore.Identity; // Pass på at du har denne using-klausulen


namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAll(); //Get all the posts from the postrepo.

            if (posts == null) //Returning error if there are no posts.
            {
                return View("Error");
            }

            posts = posts.OrderByDescending(p => p.PostTime = p.LatestComment.CommentTime);
            //Creating a new postListViewModel
            var postListViewModel = new PostListViewModel(
                posts: posts.Take(8), //the models posts is equal to posts gotten with the getall method.
                currentViewName: "Index"
            );

            //Returning the created model to the view (index)
            return View(postListViewModel);
        }
    }
}
