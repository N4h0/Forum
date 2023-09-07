namespace Forum.Models


{
	public class Post
	{
		public int PostId { get; set; } //PK
		public int ThreadId { get; set; } //FK
		public string Description { get; set; } = string.Empty;
		//Thread Thread { get; set } = Thread.Empty;



    }
}
