namespace Forum.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;//PK FK
        public string Password { get; set; } = string.Empty; //Must have a password, can not be null.
    }
}
