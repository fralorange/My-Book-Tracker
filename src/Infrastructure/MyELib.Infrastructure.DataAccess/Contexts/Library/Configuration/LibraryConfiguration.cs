using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.DataAccess.Contexts.Library.Configuration
{
    /// <summary>
    /// Конфигурация <see cref="LibraryEntity"/>
    /// </summary>
    public class LibraryConfiguration : IEntityTypeConfiguration<LibraryEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<LibraryEntity> builder)
        {
            builder.ToTable(nameof(Domain.Library.Library));

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(l => l.Documents)
                .WithOne(d => d.Library)
                .HasForeignKey(d => d.LibraryId);
        }
    }
}
