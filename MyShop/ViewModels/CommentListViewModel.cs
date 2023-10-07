using System;
using System.Collections.Generic;
using Forum.Models;

namespace Forum.ViewModels
{
    public class CommentListViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
        public string? CurrentViewName;

        public CommentListViewModel(IEnumerable<Comment> comments, string? currentViewName)
        {
            Comments = comments;
            CurrentViewName = currentViewName;
        }
    }
}