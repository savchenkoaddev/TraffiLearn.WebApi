using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.Domain.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.ServiceCenters.LocationNames;
using TraffiLearn.Domain.ServiceCenters.RoadNames;
using TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class ServiceCenterConfiguration
        : IEntityTypeConfiguration<ServiceCenter>
    {
        public void Configure(EntityTypeBuilder<ServiceCenter> builder)
        {
            builder.HasIndex(q => q.Id)
                .IsUnique();

            builder.Property(q => q.Id).HasConversion(
                 id => id.Value,
                 value => new ServiceCenterId(value));

            builder.Property(q => q.Number)
                .HasMaxLength(ServiceCenterNumber.MaxLength)
                .HasConversion(
                    content => content.Value,
                    value => ServiceCenterNumber.Create(value).Value);

            builder.OwnsOne(q => q.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.RoadName)
                    .HasMaxLength(RoadName.MaxLength)
                    .HasConversion(
                        id => id.Value,
                        value => RoadName.Create(value).Value);

                addressBuilder.Property(a => a.BuildingNumber)
                    .HasMaxLength(BuildingNumber.MaxLength)
                    .HasConversion(
                        id => id.Value,
                        value => BuildingNumber.Create(value).Value);

                addressBuilder.Property(a => a.LocationName)
                    .HasMaxLength(LocationName.MaxLength)
                    .HasConversion(
                        id => id.Value,
                        value => LocationName.Create(value).Value);
            });

            builder
                .HasOne(sc => sc.Region)
                .WithMany(r => r.ServiceCenters);

            builder
                .HasMany(sc => sc.Routes)
                .WithOne(r => r.ServiceCenter)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
