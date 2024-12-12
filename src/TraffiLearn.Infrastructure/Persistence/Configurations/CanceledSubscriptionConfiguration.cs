using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.CancelationReasons;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class CanceledSubscriptionConfiguration
        : IEntityTypeConfiguration<CanceledSubscription>
    {
        public void Configure(EntityTypeBuilder<CanceledSubscription> builder)
        {
            builder.Property(cs => cs.Id).HasConversion(
                 id => id.Value,
                 value => new CanceledSubscriptionId(value));

            builder.Property(cs => cs.UserId).HasConversion(
                 id => id.Value,
                 value => new UserId(value));

            builder.HasIndex(
                cs => cs.UserId);

            builder.Property(cs => cs.SubscriptionPlanId).HasConversion(
                 id => id.Value,
                 value => new SubscriptionPlanId(value));

            builder.Property(cs => cs.CancelationReason)
                .IsRequired(false)
                .HasMaxLength(CancelationReason.MaxLength)
                .HasConversion(
                    reason => reason.Value,
                    value => CancelationReason.Create(value).Value);
        }
    }
}
