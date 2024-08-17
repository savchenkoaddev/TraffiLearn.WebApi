using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistance.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(user => user.Id).HasConversion(
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
                .HasMany(user => user.CommentsIds)
                .WithOne(c => c.Creator);

            builder
                .HasMany(user => user.MarkedQuestionsIds)
                .WithMany()
                .UsingEntity(join => join.ToTable("QuestionsMarked"));

            builder
                .HasMany(user => user.LikedQuestionsIds)
                .WithMany(question => question.LikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsLikedByUsers"));

            builder
                .HasMany(user => user.DislikedQuestionsIds)
                .WithMany(question => question.DislikedByUsers)
                .UsingEntity(join => join.ToTable("QuestionsDislikedByUsers"));

            builder
                .HasMany(user => user.LikedCommentsIds)
                .WithMany(comment => comment.LikedByUsers)
                .UsingEntity(join => join.ToTable("CommentsLikedByUsers"));

            builder
                .HasMany(user => user.DislikedCommentsIds)
                .WithMany(comment => comment.DislikedByUsers)
                .UsingEntity(join => join.ToTable("CommentsDislikedByUsers"));
        }
    }
}
