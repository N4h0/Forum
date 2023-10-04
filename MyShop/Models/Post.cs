using System.ComponentModel.DataAnnotations;

namespace Forum.Models

	{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int TopicId { get; set; } //FK
		[Required]
		public string Description { get; set; }
		//Navigaiton property:
        public virtual Topic Topic  { get; set; } // Can't be zero. Virtual enables lazy loading. 
    }
}
