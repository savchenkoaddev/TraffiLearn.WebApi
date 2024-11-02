using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.SubscriptionPlans.PlanDescriptions;
using TraffiLearn.Domain.SubscriptionPlans.PlanFeatures;
using TraffiLearn.Domain.SubscriptionPlans.PlanTiers;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class SubscriptionPlanConfiguration
        : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasIndex(p => p.Id)
                .IsUnique();

            builder.Property(p => p.Id).HasConversion(
                id => id.Value,
                value => new SubscriptionPlanId(value));

            builder.Property(p => p.Tier)
                .HasMaxLength(PlanTier.MaxLength)
                .HasConversion(
                    tier => tier.Value,
                    value => PlanTier.Create(value).Value);

            builder.Property(p => p.Description)
                .HasMaxLength(PlanDescription.MaxLength)
                .HasConversion(
                    description => description.Value,
                    value => PlanDescription.Create(value).Value);

            builder.OwnsOne(p => p.Price, priceBuilder =>
            {
                priceBuilder.Property(price => price.Amount);
                priceBuilder.Property(price => price.Currency);
            });

            builder.OwnsOne(p => p.RenewalPeriod, periodBuilder =>
            {
                periodBuilder.Property(p => p.Interval);
                periodBuilder.Property(p => p.Type);
            });

            builder.OwnsMany(p => p.Features, featureBuilder =>
            {
                featureBuilder.ToJson();

                featureBuilder.Property(f => f.Value)
                              .HasMaxLength(PlanFeature.MaxTextLength);
            });
        }
    }
}
