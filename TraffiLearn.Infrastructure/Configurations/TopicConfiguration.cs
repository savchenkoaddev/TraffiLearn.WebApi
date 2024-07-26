using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(300);
        }
    }
}
