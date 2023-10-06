using System.ComponentModel.DataAnnotations;

namespace Forum.Models

{
    public class Comment
    {
        public int CommentId { get; set; } //PK
        public int PostId { get; set; } //FK
        [Required]
        public string CommentDescription { get; set; }
        //Navigaiton property:
        public virtual Post? Post { get; set; } //Virtual enables lazy loading.
    }
}
