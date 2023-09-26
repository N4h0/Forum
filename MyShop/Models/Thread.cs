using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Thread
	{
		public int ThreadId { get; set; } //PK
		public int RoomId { get; set; } //FK

		[Required]
		public string ThreadName { get; set; } = string.Empty; //Gir ingen mening å kunne opprette en tråd uten å gi den et navn


    }
}
