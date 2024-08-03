using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Content)
                .HasMaxLength(QuestionContent.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => QuestionContent.Create(value).Value);

            builder.Property(q => q.Explanation)
                .HasMaxLength(QuestionExplanation.MaxLength)
                .HasConversion(
                    exp => exp.Value,
                    value => QuestionExplanation.Create(value).Value);

            builder.Property(q => q.QuestionNumber)
                .HasConversion(
                    number => number.Value,
                    value => QuestionNumber.Create(value).Value);

            builder.Property(q => q.LikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.DislikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.ImageUri)
                .IsRequired(false)
                .HasMaxLength(ImageUri.MaxLength)
                .HasConversion(
                    uri => uri.Value,
                    value => ImageUri.Create(value).Value);

            builder.OwnsMany(q => q.Answers, answersBuilder =>
            {
                answersBuilder.Property(a => a.Text)
                              .HasMaxLength(Answer.MaxTextLength);

                answersBuilder.Property(a => a.IsCorrect);
            });

            builder
                .HasMany(q => q.Topics)
                .WithMany(t => t.Questions);

            builder
                .HasMany(q => q.Tickets)
                .WithMany(t => t.Questions);

            builder
                .HasMany(q => q.Comments)
                .WithOne(c => c.Question)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
