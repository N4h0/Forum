using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;

namespace Forum.Controllers
{
    public class CategoryController : Controller

    {
        private readonly ItemDbContext _itemDbContext;

        public CategoryController(ItemDbContext itemDbContext)
        {
            _itemDbContext = itemDbContext;
        }

        public IActionResult Table()
        {
            List<Category> categories = _itemDbContext.Categories.ToList();
            //var categories = GetCategories();
            var categoryListViewModel = new CategoryListViewModel(categories, "Table");
            return View(categoryListViewModel);

            // List<Category> categories = new _itemDbContext.Categories.ToList();
            //var items = new List<Category>();
            //var item1 = new Category();

            //item1.Name = "Sport";


            //var item2 = new Category
            /*{

                Name = "Mat",

            };

            items.Add(item1);
            items.Add(item2);

            ViewBag.CurrentViewName = "List of Category";
            return View(items);
        }
            */
        }


    }

}