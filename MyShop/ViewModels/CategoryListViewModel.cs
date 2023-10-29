﻿using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class CategoryListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public string? CurrentViewName;  // Name of the current view

        public CategoryListViewModel(IEnumerable<Category> categories, string? currentViewName)
        {
            Categories = categories;   // Assign the provided categories to the Categories property
            CurrentViewName = currentViewName; // Assign the provided currentViewName to the CurrentViewName property
        }
    }
}