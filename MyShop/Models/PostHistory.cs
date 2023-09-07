namespace Forum.Models
{
	public class PostHistory
	{
		public int PostId { get; set; } //PK
		public int PostID { get; set; } //FK
		public string TimeStamp { get; set; } = string.Empty; 
		public string Description { get; set; } = string.Empty; 

 	}
}
