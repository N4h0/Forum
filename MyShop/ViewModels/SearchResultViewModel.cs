using Forum.Models;

public class SearchResultViewModel
{
    public List<Category> Categories { get; set; }
    public List<Post> Posts { get; set; }

    public List<Topic> Topics { get; set; }
    public List<Room> Rooms { get; internal set; }
}
