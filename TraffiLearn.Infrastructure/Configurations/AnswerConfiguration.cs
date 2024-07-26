using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Infrastructure.Configurations
{
    internal sealed class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.Property(a => a.Text)
                .HasMaxLength(300);
        }
    }
}
