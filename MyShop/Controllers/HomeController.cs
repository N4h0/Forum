using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.DAL; // Importer riktig namespace for PostRepository
using Forum.ViewModels; // Importer riktig namespace for PostListViewModel

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAll(); // Hent alle poster fra PostRepository

            if (posts == null)
            {
                // Håndter feil her, for eksempel ved å vise en feilmelding.
                return View("Error");
            }

            var postListViewModel = new PostListViewModel(
                posts: posts,
                currentViewName: "Index"
            );

            return View(postListViewModel);
        }
    }
}
