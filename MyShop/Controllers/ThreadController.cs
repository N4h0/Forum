using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class ThreadController : Controller
    {
        private readonly CategoryDbContext _roomDbContext;

        public ThreadController(CategoryDbContext roomDbContext)
        {
            _roomDbContext = roomDbContext;
        }

        public IActionResult ThreadTable()
        {
            List<Models.Thread> threads = _roomDbContext.Threads.ToList();
            var threadListViewModel = new ThreadListViewModel(threads, "Table");
            return View(threadListViewModel);
        }

        [HttpGet]
        public IActionResult CreateThread()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateThread(Models.Thread thread)
        {
            if (ModelState.IsValid)
            {
                _roomDbContext.Threads.Add(thread);
                _roomDbContext.SaveChanges();
                return RedirectToAction(nameof(ThreadTable));
            }
            return View(thread);
        }
    }
}
