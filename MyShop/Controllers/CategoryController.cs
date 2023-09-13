using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;

namespace Forum.Controllers
{
    public class CategoryController : Controller

    {
        private readonly CategoryDbContext _categoryDbContext;

        public CategoryController(CategoryDbContext categoryDbContext)
        {
            _categoryDbContext = categoryDbContext;
        }

        public IActionResult Table()
        {
            List<Category> categories = _categoryDbContext.Categories.ToList();
            var categoryListViewModel = new CategoryListViewModel(categories, "Table");
            return View(categoryListViewModel);
        }

        public IActionResult Grid() 
        {
            List<Category> categories = _categoryDbContext.Categories.ToList();
            var categoryListViewModel = new CategoryListViewModel(categories, "Grid");
            return View(categoryListViewModel);
        }

        public IActionResult Details(int id) 
        {
            List<Category> categories = _categoryDbContext.Categories.ToList();
            var category = categories.FirstOrDefault(i => i.CategoryId == id);
            if (categories == null) 
            {
                return NotFound();
            }
            return View(categories);
        }
    }

}