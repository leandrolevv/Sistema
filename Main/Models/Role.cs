namespace Main.Models
{
    public class Role
    {
        public int Id { get; set; } = Int32.MinValue;
        public string Name { get; set; } = String.Empty;
        public string Slug { get; set; } = String.Empty;
        public List<User> Users { get; set; } = new List<User>();
    }
}
