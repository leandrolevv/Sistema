using Main.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.DbContextSistema.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(256).IsRequired();
            builder.Property(x => x.Email).HasColumnType("NVARCHAR").HasMaxLength(256).IsRequired();
            builder.Property(x => x.Slug).HasColumnType("VARCHAR").HasMaxLength(256).IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnType("VARCHAR").HasMaxLength(500).IsRequired();
            builder.Property(x => x.linkProfileImage).HasColumnType("VARCHAR").HasMaxLength(500);
            
            builder
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>("UserRole",
                    user => user.HasOne<Role>().WithMany().HasForeignKey("FK_UserId"),
                    role => role.HasOne<User>().WithMany().HasForeignKey("FK_RoleId")
                    );
        }
    }
}
