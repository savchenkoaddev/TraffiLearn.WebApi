using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Infrastructure.Persistence.Interceptors
{
    internal sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;

        public PublishDomainEventsInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;

            if (dbContext is not null)
            {
                await PublishDomainEvents(dbContext);
            }

            return result;
        }

        private async Task PublishDomainEvents(DbContext dbContext)
        {
            var entitiesWithDomainEvents = FindAllEntitiesWithDomainEvents(
                dbContext);

            var domainEvents = SelectDomainEvents(entitiesWithDomainEvents);

            ClearDomainEventsForEach(entitiesWithDomainEvents);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }

        private static List<IHasDomainEvents> FindAllEntitiesWithDomainEvents(
            DbContext dbContext)
        {
            return dbContext.ChangeTracker.Entries<IHasDomainEvents>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity)
                .ToList();
        }

        private static List<DomainEvent> SelectDomainEvents(
            List<IHasDomainEvents> entitiesWithDomainEvents)
        {
            return entitiesWithDomainEvents
                .SelectMany(entry => entry.DomainEvents)
                .ToList();
        }

        private static void ClearDomainEventsForEach(
            List<IHasDomainEvents> entitiesWithDomainEvents)
        {
            entitiesWithDomainEvents.ForEach(entity => entity.ClearDomainEvents());
        }
    }
}
