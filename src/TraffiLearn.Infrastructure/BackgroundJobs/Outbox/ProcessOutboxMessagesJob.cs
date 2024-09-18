using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.Infrastructure.Persistence.Outbox;

namespace TraffiLearn.Infrastructure.BackgroundJobs.Outbox
{
    [DisallowConcurrentExecution]
    public sealed class ProcessOutboxMessagesJob : IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublisher _publisher;
        private readonly OutboxSettings _outboxSettings;
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;

        public ProcessOutboxMessagesJob(
            ApplicationDbContext dbContext,
            IPublisher publisher,
            IOptions<OutboxSettings> outboxSettings,
            ILogger<ProcessOutboxMessagesJob> logger)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _outboxSettings = outboxSettings.Value;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _dbContext
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null)
                .Take(_outboxSettings.ProcessMessagesAtATimeCount)
                .ToListAsync(context.CancellationToken);

            foreach (OutboxMessage outboxMessage in messages)
            {
                var domainEvent = JsonConvert
                    .DeserializeObject<DomainEvent>(outboxMessage.Content);

                if (domainEvent is null)
                {
                    _logger.LogCritical("Outbox message's content is null. Id: {id}", outboxMessage.Id);

                    continue;
                }

                await _publisher.Publish(
                    domainEvent,
                    context.CancellationToken);
                
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
