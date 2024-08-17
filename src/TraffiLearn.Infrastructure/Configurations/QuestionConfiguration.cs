using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(q => q.Id).HasConversion(
                 id => id.Value,
                 value => new QuestionId(value));

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
                .HasMany(q => q.TopicsIds)
                .WithMany(t => t.Questions);

            builder
                .HasMany(q => q.TicketsIds)
                .WithMany(t => t.Questions);

            builder
                .HasMany(q => q.CommentsIds)
                .WithOne(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(q => q.LikedByUsersIds)
                .WithMany(user => user.LikedQuestions)
                .UsingEntity(join => join.ToTable("QuestionsLikedByUsers"));

            builder
                .HasMany(q => q.DislikedByUsersIds)
                .WithMany(user => user.DislikedQuestions)
                .UsingEntity(join => join.ToTable("QuestionsDislikedByUsers"));

            builder
                .Ignore(q => q.LikesCount);

            builder
                .Ignore(q => q.DislikesCount);
        }
    }
}
