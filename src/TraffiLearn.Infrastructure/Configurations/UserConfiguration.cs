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
            builder.HasKey(user => user.Id);

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

            builder.HasIndex(user => user.Email).IsUnique();

            builder
                .HasMany(user => user.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(user => user.MarkedQuestions);
        }
    }
}
