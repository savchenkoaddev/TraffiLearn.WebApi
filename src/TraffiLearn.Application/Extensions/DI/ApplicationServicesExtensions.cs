﻿using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Services;

namespace TraffiLearn.Application.Extensions.DI
{
    internal static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUsernameGenerator, UsernameGenerator>();

            return services;
        }
    }
}
