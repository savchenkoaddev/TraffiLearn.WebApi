using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Directories;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Users;
using TraffiLearn.Infrastructure.Persistence.Repositories;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IServiceCenterRepository, ServiceCenterRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IDirectoryRepository, DirectoryRepository>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ICanceledSubscriptionRepository, CanceledSubscriptionRepository>();

            return services;
        }
    }
}
