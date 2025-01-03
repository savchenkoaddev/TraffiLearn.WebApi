﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Directories;
using TraffiLearn.Domain.Directories.DirectoryNames;
using TraffiLearn.Domain.Directories.DirectorySections.SectionContents;
using TraffiLearn.Domain.Directories.DirectorySections.SectionNames;
using Directory = TraffiLearn.Domain.Directories.Directory;

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

                sectionsBuilder.Property(s => s.Name)
                    .HasConversion(
                        name => name.Value,
                        value => SectionName.Create(value).Value);

                sectionsBuilder.Property(s => s.Content)
                    .HasConversion(
                        content => content.Value,
                        value => SectionContent.Create(value).Value);
            });
        }
    }
}
