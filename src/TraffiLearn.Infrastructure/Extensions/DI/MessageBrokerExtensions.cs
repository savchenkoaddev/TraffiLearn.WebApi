﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.EventBus;
using TraffiLearn.Infrastructure.MessageBroker;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class MessageBrokerExtensions
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services)
        {
            services.ConfigureMassTransit();

            services.AddTransient<IEventBus, EventBus>();

            return services;
        }

        private static IServiceCollection ConfigureMassTransit(
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

                    configurator.ReceiveEndpoint("traffilearn-queue", endpoint => 
                    {
                        endpoint.Bind("traffilearn-exchange");
                    });

                    configurator.Message<SendEmailRequestMessage>(
                        cfg => cfg.SetEntityName("traffilearn-exchange"));
                });
            });

            return services;
        }
    }
}
