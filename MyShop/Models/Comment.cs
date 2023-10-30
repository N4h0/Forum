using System.ComponentModel.DataAnnotations;

namespace Forum.Models

{
    public class Comment
    {
        public int CommentId { get; set; } //PK
        public int PostId { get; set; } //FK

        public string? UserName { get; set; } //FK
        [Required]
        [MaxLength(10000)] // Eksempel: maximum length to 10000 character
        public string CommentDescription { get; set; }
        public DateTime CommentTime { get; set; }//Time of creation

        //Navigaiton property:
        public virtual Post? Post { get; set; } //Virtual enables lazy loading.
    }
}