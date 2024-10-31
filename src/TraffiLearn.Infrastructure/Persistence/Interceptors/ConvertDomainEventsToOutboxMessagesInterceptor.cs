using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Persistence.Outbox;

namespace TraffiLearn.Infrastructure.Persistence.Interceptors
{
    internal sealed class ConvertDomainEventsToOutboxMessagesInterceptor
        : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var entitiesWithDomainEvents = FindAllEntitiesWithDomainEvents(
                dbContext);

            var domainEvents = SelectDomainEvents(entitiesWithDomainEvents);

            var outboxMessages = ConvertDomainEventsToOutboxMessages(domainEvents);

            await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
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

        private static List<OutboxMessage> ConvertDomainEventsToOutboxMessages(
            List<DomainEvent> domainEvents)
        {
            return domainEvents
                .Select(e => new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = e.GetType().Name,
                    Content = SerializeDomainEventContent(e),
                })
                .ToList();
        }

        private static string SerializeDomainEventContent(
            DomainEvent domainEvent)
        {
            return JsonConvert.SerializeObject(
                domainEvent,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
        }
    }
}
