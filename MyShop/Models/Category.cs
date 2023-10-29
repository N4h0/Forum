
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Category
	{
		
		public int CategoryId { get; set; } //PK
        
		// Apply regular expression validation to CategoryName property
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$", ErrorMessage = "Category name must consist of letters and be between 2 and 20 characters.")]
        public string CategoryName { get; set; } = string.Empty;

		// public string? Description { get; set; } //Add this if we decide we want a description for each category. 

		public virtual List<Room>? Rooms { get; set; } //One Category has 0 or more rooms. ? Means it can have 0.
    }
}