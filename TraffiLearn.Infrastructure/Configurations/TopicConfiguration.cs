using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Number).HasConversion(
                number => number.Value,
                value => TopicNumber.Create(value));

            builder.Property(t => t.Title)
                .HasMaxLength(300)
                .HasConversion(
                title => title.Value,
                value => TopicTitle.Create(value));

            builder
                .HasMany(t => t.Questions)
                .WithMany(t => t.Topics);
        }
    }
}
