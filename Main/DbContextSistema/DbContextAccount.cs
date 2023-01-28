using Main.DbContextSistema.Mappings;
using Main.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.DbContextSistema
{
    public class DbContextAccount : DbContext
    {
        public DbContextAccount(DbContextOptions<DbContextAccount> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserMap());
            modelBuilder.ApplyConfiguration<Role>(new RoleMap());
        }
    }
}
