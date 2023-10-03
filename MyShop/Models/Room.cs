using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Room
	{
		public int RoomId { get; set; } //PK
        [Required]
        public string RoomName { get; set; } = string.Empty; //Gir ingen mening å kunne opprette et forum uten å gi det et navn
        public int CategoryId { get; set; } //FK


        //Navigaiton property:
        public virtual Category? Category { get; set; } // Can't be zero. Virtual enables lazy loading. 
		public virtual List<Topic>? Topics { get; set; } // One room can have many threads, or zero. ? means it can have zero threads.
	}
}