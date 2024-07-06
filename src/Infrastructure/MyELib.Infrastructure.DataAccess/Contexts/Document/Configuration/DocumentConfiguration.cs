using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.DataAccess.Contexts.Document.Configuration
{
    /// <summary>
    /// Конфигурация <see cref="DocumentEntity"/>
    /// </summary>
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<DocumentEntity> builder)
        {
            builder.ToTable(nameof(Domain.Document.Document));

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.Name).IsRequired().HasMaxLength(50);
            builder.Property(d => d.FileType).IsRequired();
            builder.Property(d => d.Content).IsRequired();
            builder.Property(d => d.Size).IsRequired().HasMaxLength(20000000);
            builder.Property(d => d.UploadedDate).IsRequired();
            builder.Property(d => d.UploadedBy).IsRequired();
            builder.Property(d => d.LibraryId);

            builder.HasOne(d => d.Library)
                .WithMany(l => l.Documents)
                .HasForeignKey(d => d.LibraryId);
        }
    }
}
