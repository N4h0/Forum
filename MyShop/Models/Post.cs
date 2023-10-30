using System.ComponentModel.DataAnnotations;



namespace Forum.Models

	{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int TopicId { get; set; } //FK
		public string? UserName { get; set; } //FK

        // Apply regular expression validation to PostTitle property
        [Required]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. ,\-!@#$%^&*()_+]{1,70}$", ErrorMessage = "PostTitle must be between 1 to 30 characters.")]
        public string PostTitle { get; set; }
        public DateTime PostTime { get; set; }//Time of creation"

        //Navigaiton property:
        public virtual Topic? Topic  { get; set; } 
        public virtual List<Comment>? Comments { get; set; }

        public Comment? LatestComment => Comments?.OrderByDescending(c => c.CommentId).FirstOrDefault();
    }
}
