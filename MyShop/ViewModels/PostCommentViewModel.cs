using Forum.Models;

namespace Forum.ViewModels //The PostCommentViewModel is created so that when creating a post we can force the user to create the first comment
    //At the same time, while also ensureing the Post and comments gets created at the same time.
{
    public class PostCommentViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}
