using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Infrastructure.DataAccess.Contexts.User.Configuration
{
    /// <summary>
    /// Конфигурация <see cref="UserEntity"/>
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable(nameof(Domain.User.User));

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.HashedPassword).IsRequired();
            builder.Property(u => u.Salt).IsRequired();

            builder.HasIndex(u => u.Username).IsUnique();

            builder.HasMany(u => u.LibraryUsers)
                .WithOne(lu => lu.User)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
