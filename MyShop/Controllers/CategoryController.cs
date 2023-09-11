using Forum.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class CategoryController : Controller
    {
        public IActionResult Table()
        {
            var items = new List<Category>();
            var item1 = new Category();

            item1.Name = "Sport";


            var item2 = new Category
            {

                Name = "Mat",

            };

            items.Add(item1);
            items.Add(item2);

            ViewBag.CurrentViewName = "List of Category";
            return View(items);
        }
    }
}
