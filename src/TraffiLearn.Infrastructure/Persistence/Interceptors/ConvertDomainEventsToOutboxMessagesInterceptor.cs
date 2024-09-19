using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Persistence.Outbox;

namespace TraffiLearn.Infrastructure.Persistence.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
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
                return await base.SavingChangesAsync(
                    eventData, 
                    result, 
                    cancellationToken);
            }

            var outboxMessages = ExtractOutboxMessages(dbContext);

            if (outboxMessages.Any())
            {
                await InsertOutboxMessagesAsync(
                    dbContext, 
                    outboxMessages, 
                    cancellationToken);
            }

            return await base.SavingChangesAsync(
                eventData, 
                result, 
                cancellationToken);
        }

        private static List<OutboxMessage> ExtractOutboxMessages(
            DbContext dbContext)
        {
            return dbContext.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Select(entry => entry.Entity)
                .SelectMany(ExtractAndClearDomainEvents)
                .Select(ConvertToOutboxMessage)
                .ToList();
        }

        private static IEnumerable<DomainEvent> ExtractAndClearDomainEvents(
            IHasDomainEvents entity)
        {
            var domainEvents = entity.DomainEvents.ToList();

            entity.ClearDomainEvents();

            return domainEvents;
        }

        private static OutboxMessage ConvertToOutboxMessage(
            DomainEvent domainEvent)
        {
            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            };
        }

        private static async Task InsertOutboxMessagesAsync(
            DbContext dbContext, 
            IEnumerable<OutboxMessage> outboxMessages, 
            CancellationToken cancellationToken)
        {
            await dbContext.Set<OutboxMessage>()
                .AddRangeAsync(
                    outboxMessages, 
                    cancellationToken);
        }
    }
}
