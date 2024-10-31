using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TraffiLearn.Infrastructure.BackgroundJobs;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class BackgroundJobsExtensions
    {
        public static IServiceCollection AddBackgroundJobs(
            this IServiceCollection services)
        {
            var outboxSettings = services.BuildServiceProvider().GetOptions<OutboxSettings>();

            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger => trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule => schedule
                                    .WithIntervalInSeconds(
                                        outboxSettings.ProcessIntervalInSeconds)
                                    .RepeatForever()));
            });

            services.AddQuartzHostedService();

            return services;
        }
    }
}
