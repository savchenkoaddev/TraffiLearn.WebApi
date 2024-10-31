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
                _logger.LogInformation("Retrieved {MessageCount} outbox messages to process.", messages.Count);

                await ProcessOutboxMessages(
                    messages, context.CancellationToken);

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes to the database.");
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
            if (messages.Count == 0)
            {
                _logger.LogInformation("No messages to process.");

                return;
            }

            var publishTasks = new List<Task>();

            foreach (var message in messages)
            {
                DomainEvent? domainEvent = DeserializeDomainEvent(message.Content);

                if (domainEvent is null)
                {
                    _logger.LogError("Deserialization failed for message ID {MessageId}.", message.Id);

                    continue;
                }

                _logger.LogInformation("Publishing message ID {MessageId} of type {EventType}.", message.Id, domainEvent.GetType().Name);

                var publishTask = PublishDomainEvent(
                    message, domainEvent, cancellationToken);

                publishTasks.Add(publishTask);
            }

            await Task.WhenAll(publishTasks);
            _logger.LogInformation("Completed processing of {MessageCount} outbox messages.", messages.Count);
        }

        private Task PublishDomainEvent(
            OutboxMessage message, 
            DomainEvent domainEvent, 
            CancellationToken cancellationToken)
        {
            return _publisher.Publish(
                domainEvent,
                cancellationToken)
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        message.ProcessedOnUtc = DateTime.UtcNow;

                        _logger.LogInformation("Message ID {MessageId} successfully processed.", message.Id);
                    }
                    else if (task.IsFaulted)
                    {
                        _logger.LogError(task.Exception, "Error processing message ID {MessageId}.", message.Id);
                    }
                }, cancellationToken);
        }

        private static DomainEvent? DeserializeDomainEvent(string content)
        {
            return JsonConvert
                .DeserializeObject<DomainEvent>(
                    content,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    });
        }

        private Task<List<OutboxMessage>> GetOutboxMessagesAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching outbox messages with a batch size of {BatchSize}.", _outboxSettings.BatchSize);

            return _dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .Take(_outboxSettings.BatchSize)
                .ToListAsync(cancellationToken);
        }
    }
}
