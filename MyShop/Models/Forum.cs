namespace Forum.Models
{
	public class Forum
	{
		public int ForumId { get; set; } //PK
		public string Category { get; set; } = string.Empty;   //FK
		public string ForumName { get; set; } = string.Empty;
	}
}
