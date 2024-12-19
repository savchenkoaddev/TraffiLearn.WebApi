namespace TraffiLearn.WebAPI.Extensions.DI
{
    internal static class CorsExtensions
    {
        public static readonly string DevelopmentPolicyName = "Development";

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DevelopmentPolicyName, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
