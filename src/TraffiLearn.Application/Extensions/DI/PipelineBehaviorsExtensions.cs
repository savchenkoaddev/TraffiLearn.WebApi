using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Behaviors;

namespace TraffiLearn.Application.Extensions.DI
{
    internal static class PipelineBehaviorsExtensions
    {
        public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return services;
        }
    }
}
