using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistance.Configurations
{
    internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Id).HasConversion(
                id => id.Value,
                value => new CommentId(value));

            builder.Property(c => c.Content)
                .HasMaxLength(CommentContent.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => CommentContent.Create(value).Value);

            builder
                .HasOne(c => c.QuestionId)
                .WithMany(q => q.Comments);

            builder
                .HasOne(c => c.CreatorId)
                .WithMany(q => q.Comments)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasMany(c => c.Replies)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.RootComment)
                .WithMany(c => c.Replies)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasMany(q => q.LikedByUsersIds)
                .WithMany(user => user.LikedComments)
                .UsingEntity(join => join.ToTable("CommentsLikedByUsers"));

            builder
                .HasMany(q => q.DislikedByUsersIds)
                .WithMany(user => user.DislikedComments)
                .UsingEntity(join => join.ToTable("CommentsDislikedByUsers"));

            builder
                .Ignore(q => q.LikesCount);

            builder
                .Ignore(q => q.DislikesCount);
        }
    }
}
