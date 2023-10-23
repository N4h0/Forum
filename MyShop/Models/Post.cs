using System.ComponentModel.DataAnnotations;



namespace Forum.Models

	{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int TopicId { get; set; } //FK
		public string? UserId { get; set; } //FK

		[Required]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        public string PostTitle { get; set; }
        //Navigaiton property:
        public DateTime PostTime { get; set; }// Tidspunkt for opprettelse

        public virtual Topic? Topic  { get; set; } 
        public virtual List<Comment>? Comments { get; set; }
    }
}
