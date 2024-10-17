using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Common.ImageUri;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasIndex(t => t.Id)
                .IsUnique();

            builder.Property(t => t.Id).HasConversion(
                  id => id.Value,
                  value => new TopicId(value));

            builder.Property(t => t.Number).HasConversion(
                number => number.Value,
                value => TopicNumber.Create(value).Value);

            builder.Property(t => t.Title)
                .HasMaxLength(TopicTitle.MaxLength)
                .HasConversion(
                title => title.Value,
                value => TopicTitle.Create(value).Value);

            builder
                .HasMany(t => t.Questions)
                .WithMany(t => t.Topics);

            builder.Property(q => q.ImageUri)
                .IsRequired(false)
                .HasMaxLength(ImageUri.MaxLength)
                .HasConversion(
                    uri => uri.Value,
                    value => ImageUri.Create(value).Value);
        }
    }
}
