using Main.DbContextSistema.Mappings;
using Main.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.DbContextSistema
{
    public class DbContextAccount : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Sistema;User ID=sa;Password=123456;Trusted_Connection=False; TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserMap());
            modelBuilder.ApplyConfiguration<Role>(new RoleMap());
        }
    }
}
