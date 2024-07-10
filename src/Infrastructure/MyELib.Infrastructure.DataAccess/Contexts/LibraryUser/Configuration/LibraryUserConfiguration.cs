using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyELib.Domain.Identity;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Infrastructure.DataAccess.Contexts.LibraryUser.Configuration
{
    /// <summary>
    /// Конфигурация <see cref="LibraryUserEntity"/>
    /// </summary>
    public class LibraryUserConfiguration : IEntityTypeConfiguration<LibraryUserEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<LibraryUserEntity> builder)
        {
            builder.ToTable(nameof(Domain.LibraryUser.LibraryUser));

            builder.HasKey(lu => lu.Id);

            builder.Property(lu => lu.Id).ValueGeneratedOnAdd();
            builder.Property(lu => lu.LibraryId).IsRequired();
            builder.Property(lu => lu.UserId).IsRequired();
            builder.Property(lu => lu.Role)
                .IsRequired()
                .HasConversion(
                    ar => ar.ToString(),
                    ar => (AuthRoles)Enum.Parse(typeof(AuthRoles), ar));

            builder.HasOne(lu => lu.Library)
                .WithMany(l => l.LibraryUsers)
                .HasForeignKey(lu => lu.LibraryId);

            builder.HasOne(lu => lu.User)
                .WithMany(u => u.LibraryUsers)
                .HasForeignKey(lu => lu.UserId);
        }
    }
}
