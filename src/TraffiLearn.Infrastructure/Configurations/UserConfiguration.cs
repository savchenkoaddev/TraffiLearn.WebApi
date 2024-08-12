using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(q => q.Id).HasConversion(
                 id => id.Value,
                 value => new UserId(value));

            builder.Property(user => user.Username)
                .HasMaxLength(Username.MaxLength)
                .HasConversion(
                    username => username.Value,
                    value => Username.Create(value).Value);

            builder.Property(user => user.Email)
                .HasMaxLength(Email.MaxLength)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value);

            builder
                .HasIndex(user => user.Email)
                .IsUnique();

            builder
                .HasIndex(user => user.Username)
                .IsUnique();

            builder
                .HasMany(user => user.Comments)
                .WithOne(c => c.Creator)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(user => user.MarkedQuestions)
                .WithMany()
                .UsingEntity(join => join.ToTable("QuestionsMarked"));

            builder
                .HasMany(user => user.LikedQuestions)
                .WithMany(question => question.LikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsLikedByUsers"));

            builder
                .HasMany(user => user.DislikedQuestions)
                .WithMany(question => question.DislikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsDislikedByUsers"));
        }
    }
}
