namespace Main.Models
{
    public class User
    {
        public int Id { get; set; } = Int32.MinValue;
        public string Slug { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string linkProfileImage { get; set; } = String.Empty;
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
