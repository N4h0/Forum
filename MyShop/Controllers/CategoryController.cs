//Getting directives.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Forum.DAL;

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

        //Method for the category table page.
        public async Task<IActionResult> CategoryTable()
        {//Getting all categories from the repository.
            var categories = await _categoryRepository.GetAll(); 
            if (categories == null) //If there are no categories found, log an error and returning NotFound.
            {
                _logger.LogError("[CategoryController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            //Creating a listviewmodel with the categories .
            var categoryListViewModel = new CategoryListViewModel(categories, "Table");
            return View(categoryListViewModel); //Returning the categoryListViewModel to the view.
        }
        //Method for the categorygrid, pretty much equal to the category table method.
        public async Task<IActionResult> CategoryGrid()
        {
            var categories = await _categoryRepository.GetAll();
            if (categories == null)//If there are no categories found, log an error and return NotFound.
            {
                _logger.LogError("[CategoryController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            //Creating a listviewmodel with the categories .
            var categoryListViewModel = new CategoryListViewModel(categories, "Grid");
            return View(categoryListViewModel);
        }
        //Method for the controller for categorydetails, which is a page for one given category
        public async Task<IActionResult> CategoryDetails(int Id) 
        {
            _logger.LogInformation("[CategoryController] Id passed to categorydetails: {Id}.", Id); //Logging the passed Id
            //Using a DAL method to find a category based on it's Id
            var category = await _categoryRepository.GetCategoryById(Id); 
            if (category == null) //Error message if category is null.
            {
                _logger.LogError("[CategoryController] Category not found for the categoryId {Id}.", Id);
                return NotFound("Category not found for the CategoryId");
            }
            return View(category); //Returning to category/categorydetails with the created category.
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only a superadmin can access the createcategory get method
        public IActionResult CreateCategory()
        {
            return View(); //Returns the Category view view
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only a superadmin can access the createcategory post method
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid) //Checking if the modelstate is valid (based on requirements found under the model folder
            {
                await _categoryRepository.Create(category); //Passing the category to DAL to create it
                return RedirectToAction(nameof(CategoryTable)); //Redirect to XXX/fotball (Or whatever other category was created).
            }
            _logger.LogWarning("[CategoryController] Category creation failed {@category}", category);
            return View(category); //returning the createCategory view with the category
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only superadmin can access this method
        public async Task<IActionResult> UpdateCategory(int id) //Method to update a category
        {
            var category = await _categoryRepository.GetCategoryById(id); //getting a category based on id
            if (category == null)
            {
                _logger.LogError("[CategoryController] Category not found when updating the CategoryId {id}", id);
                return BadRequest("Category not found for the CategoryId");
            }
            return View(category); //Returning the UpdateCategory view with the passed category.
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //only superadmin can access this method.
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (ModelState.IsValid) //Checking if the method stat is valid based on the model found under Models.
            {
                bool returnOk = await _categoryRepository.Update(category); //Updating category (method in CategoryRepository).
                if (returnOk) //If a true get's passed the client get's redirected to Category/Categorytable.
                    return RedirectToAction(nameof(CategoryTable));
            }
            //logging error, which happens if a true is not returned from the DAL
            _logger.LogWarning("[CategoryController] Category update failed {@category}", category);
            //Returning the category to the UpdateCategory view.
            return View(category); 
        }



        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin.
        public async Task<IActionResult> DeleteCategory(int Id) //Deleting a category based on Id.
        {
            var category = await _categoryRepository.GetCategoryById(Id); //Getting category based on id.
            if (category == null) //checking if the category is null. Logging that the passed if if the category is null.
            {
                _logger.LogError("[CategoryController] Item not found for the CategoryId {Id}", Id);
                return BadRequest("Category not found for the CategoryId"); //returned to erropage.
            }
            return View(category); //Returning to Deletecategory with the category we want to delete passed.
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin.
        public async Task<IActionResult> DeleteConfirmedCategory(int Id) //Post method to delete a passed category.
        {
            bool returnOk = await _categoryRepository.Delete(Id); //Passing the category to the DAL to be deleted.
            if (!returnOk) //If the return is not okay, we log the error and return a bad request.
            {

                _logger.LogError("[CategoryController] Category deletion failed for the CategoryId {Id}", Id);
                return BadRequest("Category deletion failed");
            }
            return RedirectToAction(nameof(CategoryTable)); //Redirecting to Category/Categorytable.
        }
    }
}