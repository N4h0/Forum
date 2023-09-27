using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Thread
	{
		public int ThreadId { get; set; } //PK
		public int RoomId { get; set; } //FK

		[Required]
		public string ThreadName { get; set; } = string.Empty; //Gir ingen mening å kunne opprette en tråd uten å gi den et navn

        //Navigaiton property:

        public virtual Room room { get; set; } // Can't be zero. Virtual enables lazy loading. 

        public virtual List<Post>? Posts { get; set; } // O
    }
}
