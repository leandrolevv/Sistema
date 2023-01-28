using Main.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Main.DbContextSistema.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(256).IsRequired();
            builder.Property(x => x.Slug).HasColumnType("VARCHAR").HasMaxLength(256).IsRequired();
            builder.HasIndex(x => x.Slug,"IDX_ROLE_SLUG").IsUnique();
        }
    }
}
