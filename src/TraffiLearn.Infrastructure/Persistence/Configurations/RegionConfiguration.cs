using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Regions.RegionNames;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasIndex(r => r.Id)
                .IsUnique();

            builder.Property(r => r.Id).HasConversion(
                id => id.Value,
                value => new RegionId(value));

            builder.Property(r => r.Name)
                .HasMaxLength(RegionName.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => RegionName.Create(value).Value);

            builder
                .HasMany(r => r.ServiceCenters)
                .WithOne(sc => sc.Region)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
