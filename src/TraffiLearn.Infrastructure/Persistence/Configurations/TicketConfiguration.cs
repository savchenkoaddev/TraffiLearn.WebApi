using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Configurations
{
    internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasIndex(t => t.Id)
                .IsUnique();

            builder.Property(t => t.Id).HasConversion(
                 id => id.Value,
                 value => new TicketId(value));

            builder.Property(t => t.TicketNumber).HasConversion(
                    ticketNumber => ticketNumber.Value,
                    value => TicketNumber.Create(value).Value);

            builder
                .HasMany(t => t.Questions)
                .WithMany(t => t.Tickets);
        }
    }
}
