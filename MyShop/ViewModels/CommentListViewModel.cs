using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class CommentListViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
        public Dictionary<int, List<Topic>> TopicsByComment { get; set; }
        public string? CurrentViewName;

        public CommentListViewModel(IEnumerable<Comment> Comments, Dictionary<int, List<Topic>> topicsByComment, string? currentViewName)
        {
            Comments = Comments;
            TopicsByComment = topicsByComment;
            CurrentViewName = currentViewName;
        }

        public CommentListViewModel(IEnumerable<Comment> Comments)
        {
            Comments = Comments;
        }
    }
}