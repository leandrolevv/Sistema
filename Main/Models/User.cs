namespace Main.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Slug { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string linkProfileImage { get; set; } = String.Empty;
        public IList<Role> Roles { get; set; } = new List<Role>();
    }
}
