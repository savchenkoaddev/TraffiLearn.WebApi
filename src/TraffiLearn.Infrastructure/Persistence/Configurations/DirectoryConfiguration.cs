using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Directories;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class DirectoryConfiguration : IEntityTypeConfiguration<Directory>
    {
        public void Configure(EntityTypeBuilder<Directory> builder)
        {
            builder.HasIndex(d => d.Id)
                .IsUnique();

            builder.Property(d => d.Id).HasConversion(
                id => id.Value,
                value => new DirectoryId(value));

            builder.Property(d => d.Name)
                .HasMaxLength(DirectoryName.MaxLength)
                .HasConversion(
                    name => name.Value,
                    value => DirectoryName.Create(value).Value);

            builder.OwnsMany(d => d.Sections, sectionsBuilder =>
            {
                sectionsBuilder.ToJson();
            });
        }
    }
}
