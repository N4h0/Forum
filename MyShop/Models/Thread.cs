namespace Forum.Models
{
	public class Thread
	{
		public int ThreadId { get; set; } //PK
		public int ForumId { get; set; } //FK
		public string ForumName { get; set; } = string.Empty;
	}
}
