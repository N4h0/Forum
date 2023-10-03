namespace Forum.Models


{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int TopicId { get; set; } //FK
		public string Description { get; set; } = string.Empty;
		//Navigaiton property:
        public virtual Topic Topic  { get; set; } // Can't be zero. Virtual enables lazy loading. 

        public virtual List<PostHistory>? Posthistories { get; set; } // O


    }
}
