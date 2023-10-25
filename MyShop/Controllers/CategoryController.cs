using System;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IActionResult> CategoryTable()


        {
            var categories = await _categoryRepository.GetAll();
            if (categories == null)
            {
                _logger.LogError("[CategoryController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            var categoryListViewModel = new CategoryListViewModel(categories, "Table");
            return View(categoryListViewModel);
        }

        public async Task<IActionResult> CategoryGrid()
        {

            var categories = await _categoryRepository.GetAll();
            if (categories == null)
            {
                _logger.LogError("[CategoryController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            var categoryListViewModel = new CategoryListViewModel(categories, "Grid");
            return View(categoryListViewModel);
        }

        public async Task<IActionResult> CategoryDetails(int Id)
        {
            _logger.LogInformation("[CategoryController] ID: {Id}.", Id);
            var category = await _categoryRepository.GetCategoryById(Id);
            _logger.LogInformation("[CategoryController] ID passed to CategoryController after database operation: {Id}.", Id);
            if (category == null)
            {
                _logger.LogError("[CategoryController] Category not found for the categoryId {Id}.", Id);
                return NotFound("Category not found for the CategoryId");
            }
            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.Create(category);
                return RedirectToAction(nameof(CategoryTable));
            }
            _logger.LogWarning("[CategoryController] Category creation failed {@category}", category);
            return View(category);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                _logger.LogError("[CategoryController] Category not found when updating the CategoryId {id}", id);
                return BadRequest("Category not found for the CategoryId");
            }
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _categoryRepository.Update(category);
                if (returnOk)
                    return RedirectToAction(nameof(CategoryTable));
            }
            _logger.LogWarning("[CategoryController] Category update failed {@category}", category);
            return View(category);
        }



        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                _logger.LogError("[CategoryController] Item not found for the CategoryId {CategoryId:0000}", id);
                return BadRequest("Category not found for the CategoryId");
            }
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmedCategory(int id)
        {
            bool returnOk = await _categoryRepository.Delete(id);
            if (!returnOk)
            {

                _logger.LogError("[CategoryController] Category deletion failed for the CategoryId {CategoryId:0000}", id);
                return BadRequest("Category deletion failed");
            }
            return RedirectToAction(nameof(CategoryTable));
        }

        [HttpGet]
        public async Task<IActionResult> FindCategory(string search)
        {
            ViewData["GetCategoryDetails"] = search;
            var categories = await _categoryRepository.GetAll();

            var categoryquery = from x in categories select x;
            if (!string.IsNullOrEmpty(search))
            {
                categoryquery = categoryquery.Where(x => x.CategoryName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Legg til midlertidige logger for feilsøking
            foreach (var category in categoryquery)
            {
                Console.WriteLine($"Category ID: {category.CategoryId}, Name: {category.CategoryName}");
            }

            return View(categoryquery);
        }

    }
}