using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class TopicConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Topics.Topic>
    {
        public void Configure(EntityTypeBuilder<Domain.Aggregates.Topics.Topic> builder)
        {
            builder.Property((System.Linq.Expressions.Expression<Func<Domain.Aggregates.Topics.Topic, Domain.Aggregates.Topics.ValueObjects.TopicId>>)(q => (Domain.Aggregates.Topics.ValueObjects.TopicId)q.Id)).HasConversion(
                  id => id.Value,
                  value => new Domain.Aggregates.Topics.ValueObjects.TopicId(value));

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
        }
    }
}
