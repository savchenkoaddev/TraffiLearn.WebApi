using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Content)
                .HasMaxLength(2000)
                .HasConversion(
                    content => content.Value,
                    value => QuestionContent.Create(value));

            builder.Property(q => q.Explanation)
                .HasMaxLength(2000)
                .HasConversion(
                    exp => exp.Value,
                    value => QuestionExplanation.Create(value));

            builder.Property(q => q.TicketNumber)
                .HasConversion(
                    number => number.Value,
                    value => TicketNumber.Create(value));

            builder.Property(q => q.QuestionNumber)
                .HasConversion(
                    number => number.Value,
                    value => QuestionNumber.Create(value));

            builder.Property(q => q.LikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.DislikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.ImageUri)
                .IsRequired(false)
                .HasMaxLength(300)
                .HasConversion(
                    uri => uri.Value,
                    value => ImageUri.Create(value));

            builder.OwnsMany(q => q.Answers, answersBuilder =>
            {
                answersBuilder.Property(a => a.Text)
                              .HasMaxLength(300);

                answersBuilder.Property(a => a.IsCorrect);
            });

            builder 
                .HasMany(q => q.Topics)
                .WithMany(q => q.Questions);
        }
    }
}
