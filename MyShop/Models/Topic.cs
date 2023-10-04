using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Topic
	{
		public int TopicId { get; set; } //PK
		public int RoomId { get; set; } //FK

		[Required]
		public string TopicName { get; set; } = string.Empty; //Gir ingen mening å kunne opprette en tråd uten å gi den et navn

        public string Description { get; set; } = string.Empty;

        //Navigaiton property:

        public virtual Room room { get; set; } // Can't be zero. Virtual enables lazy loading. 

        public virtual List<Post>? Posts { get; set; }
    }
}
