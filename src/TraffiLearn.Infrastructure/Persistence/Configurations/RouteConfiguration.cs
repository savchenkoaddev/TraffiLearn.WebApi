using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Common.ImageUri;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.Routes.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    public sealed class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasIndex(route => route.Id)
                .IsUnique();

            builder.Property(route => route.Id).HasConversion(
                id => id.Value,
                value => new RouteId(value));

            builder.Property(route => route.RouteNumber)
                .HasConversion(
                    routeNumber => routeNumber.Value,
                    value => RouteNumber.Create(value).Value);

            builder.Property(route => route.Description)
                .IsRequired(false)
                .HasMaxLength(RouteDescription.MaxLength)
                .HasConversion(
                    description => description.Value,
                    value => RouteDescription.Create(value).Value);

            builder.Property(route => route.ImageUri)
                .HasMaxLength(ImageUri.MaxLength)
                .HasConversion(
                    uri => uri.Value,
                    value => ImageUri.Create(value).Value);

            builder
                .HasOne(r => r.ServiceCenter)
                .WithMany(sc => sc.Routes);
        }
    }
}
