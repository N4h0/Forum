using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            var category = _categoryDbContext.Categories.FirstOrDefault(i => i.CategoryId == id);
            if (category == null) 
            {
                return NotFound();
            }
            return View(category);
        }


    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _categoryDbContext.Categories.Add(category);
            _categoryDbContext.SaveChanges();
                return RedirectToAction(nameof(Table));
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult UpdateCategory(int id)
        {
            var item = _categoryDbContext.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

    [HttpPost]
    public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryDbContext.Categories.Update(category);
                _categoryDbContext.SaveChanges();
                return RedirectToAction(nameof(Table));
            }
            return View(category);
        }



    [HttpGet]
    public IActionResult DeleteCategory(int id)
        {
            var item = _categoryDbContext.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

    [HttpPost]
    public IActionResult DeleteConfirmedCategory(int id)
    {
        var item = _categoryDbContext.Categories.Find(id);
        if (item == null)
        {
            return NotFound();
        }
            _categoryDbContext.Categories.Remove(item);
            _categoryDbContext.SaveChanges();
            return RedirectToAction(nameof(Table));
    }
}
}