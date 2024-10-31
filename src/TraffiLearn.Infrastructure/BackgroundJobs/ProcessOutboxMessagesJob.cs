using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.Infrastructure.Persistence.Options;
using TraffiLearn.Infrastructure.Persistence.Outbox;

namespace TraffiLearn.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    internal sealed class ProcessOutboxMessagesJob : IJob
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
            try
            {
                var messages = await GetOutboxMessagesAsync(
                    context.CancellationToken);

                await ProcessOutboxMessages(
                    messages, context.CancellationToken);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error occurred while saving changes.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "Unhandled exception occurred in job execution.");
                throw;
            }
        }

        private async Task ProcessOutboxMessages(
            List<OutboxMessage> messages,
            CancellationToken cancellationToken)
        {
            foreach (var message in messages)
            {
                DomainEvent? domainEvent = JsonConvert
                    .DeserializeObject<DomainEvent>(message.Content);

                if (domainEvent is null)
                {
                    continue;
                }

                await _publisher.Publish(
                    domainEvent,
                    cancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
        }

        private Task<List<OutboxMessage>> GetOutboxMessagesAsync(
            CancellationToken cancellationToken)
        {
            return _dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .Take(_outboxSettings.BatchSize)
                .ToListAsync(cancellationToken);
        }
    }
}
