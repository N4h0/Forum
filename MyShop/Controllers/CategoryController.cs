using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Forum.Controllers
{
    public class CategoryController : Controller

    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

       

        public async Task< IActionResult> CategoryTable()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryListViewModel = new CategoryListViewModel(categories, "Table");
            return View(categoryListViewModel);
        }

        public async Task< IActionResult> CategoryGrid() 
        {

            var categories = await _categoryRepository.GetAll();
            var categoryListViewModel = new CategoryListViewModel(categories, "Grid");
            return View(categoryListViewModel);
        }

        public async Task<IActionResult> CategoryDetails(int id) 
        {

            var category = await _categoryRepository.GetItemById( id);
            if (category == null) 
            {
                return BadRequest("Item not found. ");
            }
            return View(category);
        }


    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
                await _categoryRepository.Create(category);
                return RedirectToAction(nameof(CategoryTable));
            }
        return View(category);
    }

    [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryRepository.GetItemById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

    [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.Update(category);
                return RedirectToAction(nameof(CategoryTable));
            }
            return View(category);
        }



    [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetItemById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

    [HttpPost]
        public async Task<IActionResult> DeleteConfirmedCategory(int id)
    {
            await _categoryRepository.Delete(id);
            return RedirectToAction(nameof(CategoryTable));
        }
}
}