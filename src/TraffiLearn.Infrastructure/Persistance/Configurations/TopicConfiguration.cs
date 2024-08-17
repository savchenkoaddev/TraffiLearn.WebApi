using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistance.Configurations
{
    internal sealed class TopicConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Topics.Topic>
    {
        public void Configure(EntityTypeBuilder<Domain.Aggregates.Topics.Topic> builder)
        {
            builder.Property(q => q.Id).HasConversion(
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
                .HasMany(t => t.QuestionsIds)
                .WithMany(t => t.Topics);
        }
    }
}
