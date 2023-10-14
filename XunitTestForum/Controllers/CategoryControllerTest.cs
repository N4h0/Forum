using Forum.Controllers;
using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitTestForum.Controllers
{
    public class CaregoryControllerTests
    {
        [Fact]
        public async Task TestTable()
        {
            // arrange
            var categoryList = new List<Category>
    {
        new Category
        {
            CategoryId = 1,
            Name = "Sport"
        },
        new Category
        {
            CategoryId = 2,
            Name = "Data",
        }
    };
            var mockItemRepository = new Mock<CategoryRepository>();
            mockItemRepository.Setup(repo => repo.GetAll()).ReturnsAsync(categoryList);
            var mockLogger = new Mock<ILogger<CategoryController>>();
            var categoryController = new CategoryController(mockItemRepository.Object, mockLogger.Object);

            // act
            var result = await categoryController.CategoryTable();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var categoryListViewModel = Assert.IsAssignableFrom<CategoryListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, categoryListViewModel.Categories.Count());
            Assert.Equal(categoryList, categoryListViewModel.Categories);
        }

        [Fact]
        public async Task TestCreateNotOk()
        {
            // arrange
            var testCategory = new Category
            {
                CategoryId = 1,
                Name = "sport",
            };
            var mockItemRepository = new Mock<CategoryRepository>();
            mockItemRepository.Setup(repo => repo.Create(testCategory)).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<CategoryController>>();
            var categoryController = new CategoryController(mockItemRepository.Object, mockLogger.Object);

            // act
            var result = await categoryController.CreateCategory(testCategory);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewCategory = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(testCategory, viewCategory);
        }
    }
}