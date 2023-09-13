namespace Forum.Models
{
	public class PostHistory
	{
		public int PostHistoryId { get; set; } //PK
		public int ThreadID { get; set; } //FK
		public string TimeStamp { get; set; } = string.Empty; 
		public string Description { get; set; } = string.Empty; 

 	}
}
