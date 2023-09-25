namespace Forum.Models
{
	public class Category
	{
		
		public int CategoryId { get; set; } //PK
		public string Name { get; set; } = string.Empty;

		// public string? Description { get; set; } //Add this if we decide we want a description for each category. 

		public virtual List<Room>? Rooms { get; set; } //One Category has 0 or more rooms. ? Means it can have 0.
    }
}