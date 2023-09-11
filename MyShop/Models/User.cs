namespace Forum.Models
{
    public class User
    {
        public string username { get; set; } //PK
        public string password { get; set; } //Must have a password, can not be null.
    }
}
