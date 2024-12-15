using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TraffiLearn.Infrastructure.MessageBroker;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class MessageBrokerExtensions
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    MessageBrokerSettings settings = context
                        .GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                    configurator.Host(new Uri(settings.Host), h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });

                    configurator.UseMessageRetry(r => r.Interval(
                        settings.RetryCount, settings.RetryIntervalMilliseconds));
                });
            });

            return services;
        }
    }
}
