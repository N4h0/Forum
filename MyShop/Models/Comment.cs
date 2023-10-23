using System.ComponentModel.DataAnnotations;

namespace Forum.Models

{
    public class Comment
    {
        public int CommentId { get; set; } //PK
        public int PostId { get; set; } //FK

        public string? UserId { get; set; } //FK
        [Required]
        [MaxLength(500)] // Eksempel: maximum length to 500 character
        [RegularExpression("^[a-zA-Z0-9Ê¯Â∆ÿ≈.,!?\\s]*$", ErrorMessage = "Comment contains invalid characters.")]
        public string CommentDescription { get; set; }
        //Navigaiton property:
        public virtual Post? Post { get; set; } //Virtual enables lazy loading.
    }
}