﻿using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class CategoryListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public string? CurrentViewName;

        public CategoryListViewModel(IEnumerable<Category> categories, string? currentViewName)
        {
            Categories = categories;
            CurrentViewName = currentViewName;
        }
    }
}