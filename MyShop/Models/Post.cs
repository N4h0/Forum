using System.ComponentModel.DataAnnotations;

namespace Forum.Models

	{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int TopicId { get; set; } //FK
		[Required]
		public string PostTitle { get; set; }
        //Navigaiton property:

        public virtual Topic? Topic  { get; set; } 
        public virtual List<Comment>? Comments { get; set; }
    }
}
