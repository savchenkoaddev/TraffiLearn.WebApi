﻿using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Images.Options;
using TraffiLearn.Application.UseCases.Questions.Options;
using TraffiLearn.Infrastructure.Authentication.Options;
using TraffiLearn.Infrastructure.BackgroundJobs.Options;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.External.GoogleAuth.Options;
using TraffiLearn.Infrastructure.External.GroqAI.Options;
using TraffiLearn.Infrastructure.MessageBroker;
using TraffiLearn.Infrastructure.Persistence.Options;
using TraffiLearn.Infrastructure.RateLimiting.Options;
using TraffiLearn.Infrastructure.Services.Emails.Options;
using TraffiLearn.Infrastructure.Services.Payments.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class AppOptionsExtensions
    {
        public static IServiceCollection AddAppOptions(
            this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(
                DbSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<AzureBlobStorageSettings>(
                AzureBlobStorageSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<JwtSettings>(
                JwtSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<QuestionsSettings>(
                QuestionsSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<ImageSettings>(
                ImageSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<GroqApiSettings>(
                GroqApiSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<SmtpClientSettings>(
                SmtpClientSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<EmailLinkGeneratorSettings>(
                EmailLinkGeneratorSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<GoogleAuthSettings>(
                GoogleAuthSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<OutboxSettings>(
                OutboxSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<StripeSettings>(
                StripeSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<MessageBrokerSettings>(
                MessageBrokerSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<PlanExpiryNotificationSettings>(
                PlanExpiryNotificationSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<RateLimitingSettings>(
                RateLimitingSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<DefaultRateLimitingPolicySettings>(
                DefaultRateLimitingPolicySettings.SectionName);

            return services;
        }
    }
}
