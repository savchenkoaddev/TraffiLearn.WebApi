using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.OwnsMany(
                question => question.Answers, 
                ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                });


            builder.OwnsOne(q => q.QuestionTitleDetails);

            builder.Property(q => q.Content)
                .HasMaxLength(2000);

            builder.Property(q => q.Explanation)
                .HasMaxLength(2000);

            builder.Property(q => q.LikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.DislikesCount)
                .HasDefaultValue(0);

            builder.Property(q => q.ImageUri)
                .IsRequired(required: false)
                .HasMaxLength(300);
        }
    }
}
