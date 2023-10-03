using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class PostListViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public string? CurrentViewName;

        public PostListViewModel(IEnumerable<Post> posts, string? currentViewName)
        {
          Posts = posts;
            CurrentViewName = currentViewName;
        }
    }
}