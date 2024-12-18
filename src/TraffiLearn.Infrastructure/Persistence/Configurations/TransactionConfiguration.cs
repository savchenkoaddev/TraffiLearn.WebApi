using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Transactions;
using TraffiLearn.Domain.Transactions.Metadatas;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.Id).HasConversion(
                 id => id.Value,
                 value => new TransactionId(value));

            builder
                .HasOne(t => t.User)
                .WithMany(user => user.Transactions);

            builder
                .HasOne(t => t.SubscriptionPlan)
                .WithMany();

            builder.Property(p => p.Metadata)
                .IsRequired(false)
                .HasMaxLength(Metadata.MaxLength)
                .HasConversion(
                    m => m.Value,
                    value => Metadata.Create(value).Value);
        }
    }
}
