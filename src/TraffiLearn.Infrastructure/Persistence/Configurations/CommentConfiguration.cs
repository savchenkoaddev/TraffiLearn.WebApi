using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.CommentContents;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasIndex(c => c.Id)
                .IsUnique();

            builder.Property(c => c.Id).HasConversion(
                id => id.Value,
                value => new CommentId(value));

            builder.Property(c => c.Content)
                .HasMaxLength(CommentContent.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => CommentContent.Create(value).Value);

            builder
                .HasOne(c => c.Question)
                .WithMany(q => q.Comments);

            builder
                .HasOne(c => c.Creator)
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
                .HasMany(q => q.LikedByUsers)
                .WithMany(user => user.LikedComments)
                .UsingEntity(join => join.ToTable("CommentsLikedByUsers"));

            builder
                .HasMany(q => q.DislikedByUsers)
                .WithMany(user => user.DislikedComments)
                .UsingEntity(join => join.ToTable("CommentsDislikedByUsers"));

            builder
                .Ignore(q => q.LikesCount);

            builder
                .Ignore(q => q.DislikesCount);
        }
    }
}
