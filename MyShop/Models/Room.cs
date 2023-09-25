namespace Forum.Models
{
	public class Room
	{
		public int RoomId { get; set; } //PK
		public string Category { get; set; } = string.Empty;   //FK
		public string RoomName { get; set; } = string.Empty;

		//Navigaiton property:

		//public virtual Category category { get; set; } // Kan ikke være null 

		//public virtual List<Thread>? threads { get; set; }
	}
}
