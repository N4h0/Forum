namespace Forum.Models
{
	public class Room
	{
		public int RoomId { get; set; } //PK
		public string RoomName { get; set; } = string.Empty;

        public int CategoryId { get; set; } //FK


        //Navigaiton property:

        public virtual Category Category { get; set; } // Can't be zero. Virtual enables lazy loading. 

		public virtual List<Thread>? Threads { get; set; } // One room can have many threads, or zero. ? means it can have zero threads.
	}
}