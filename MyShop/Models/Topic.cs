using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Topic
	{
		public int TopicId { get; set; } //PK
		public int RoomId { get; set; } //FK

        // Apply regular expression validation to TopicName property

        [Required]
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{1,20}$", ErrorMessage = "The name must consist of  letters and be between 1 and 20 characters in length..")]
        public string TopicName { get; set; } = string.Empty; //"It doesn't make sense to be able to create a thread without giving it a name."


        [RegularExpression(@"^[\w\s\d.,!?()-]{2,200}$", ErrorMessage = "The description must be between 2 to 200 characters and can include letters, numbers, spaces, and common punctuation.")]
        public string Description { get; set; } = string.Empty;
        //Navigaiton property:

        public virtual Room? Room { get; set; } // Virtual enables lazy loading. 

        public virtual List<Post>? Posts { get; set; }
    }
}
