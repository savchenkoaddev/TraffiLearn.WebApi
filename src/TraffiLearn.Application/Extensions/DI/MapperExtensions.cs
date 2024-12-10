using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.UseCases.Auth.Commands.RegisterUser;
using TraffiLearn.Application.UseCases.Auth.Mappers;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Application.UseCases.Comments.Mappers;
using TraffiLearn.Application.UseCases.Directories.Commands.Create;
using TraffiLearn.Application.UseCases.Directories.Commands.Update;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.Application.UseCases.Directories.Mappers;
using TraffiLearn.Application.UseCases.Questions.Commands.Create;
using TraffiLearn.Application.UseCases.Questions.Commands.Update;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Questions.Mappers;
using TraffiLearn.Application.UseCases.Regions.Commands.Create;
using TraffiLearn.Application.UseCases.Regions.DTO;
using TraffiLearn.Application.UseCases.Regions.Mappers;
using TraffiLearn.Application.UseCases.Routes.Commands.Create;
using TraffiLearn.Application.UseCases.Routes.Commands.Update;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.Application.UseCases.Routes.Mappers;
using TraffiLearn.Application.UseCases.ServiceCenters.Commands.Create;
using TraffiLearn.Application.UseCases.ServiceCenters.Commands.Update;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.Application.UseCases.ServiceCenters.Mappers;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Create;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Update;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Mappers;
using TraffiLearn.Application.UseCases.Tickets.Commands.Create;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Application.UseCases.Tickets.Mappers;
using TraffiLearn.Application.UseCases.Topics.Commands.Create;
using TraffiLearn.Application.UseCases.Topics.Commands.Update;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.Application.UseCases.Topics.Mappers;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Application.UseCases.Users.Mappers;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Regions;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.Domain.SubscriptionPlans;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Application.Extensions.DI
{
    internal static class MapperExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddMapper<Question, QuestionResponse,
                QuestionToQuestionResponseMapper>();
            services.AddMapper<Topic, TopicResponse, TopicToTopicResponseMapper>();
            services.AddMapper<CreateTopicCommand, Result<Topic>,
                CreateTopicCommandMapper>();
            services.AddMapper<CreateQuestionCommand, Result<Question>,
                CreateQuestionCommandMapper>();
            services.AddMapper<CreateTicketCommand, Result<Ticket>, CreateTicketCommandMapper>();
            services.AddMapper<Ticket, TicketResponse,
                TicketToTicketResponseMapper>();
            services.AddMapper<RegisterUserCommand, Result<User>,
                RegisterUserCommandMapper>();
            services.AddMapper<UpdateQuestionCommand, Result<Question>,
                UpdateQuestionCommandMapper>();
            services.AddMapper<UpdateTopicCommand, Result<Topic>,
                UpdateTopicCommandMapper>();
            services.AddMapper<Comment, CommentResponse,
                CommentToCommentResponseMapper>();
            services.AddMapper<RegisterAdminCommand, Result<User>, RegisterAdminCommandMapper>();
            services.AddMapper<User, ApplicationUser, UserToApplicationUserMapper>();
            services.AddMapper<User, UserResponse, UserToUserResponseMapper>();
            services.AddMapper<Region, RegionResponse, RegionToRegionResponseMapper>();
            services.AddMapper<CreateRegionCommand, Result<Region>, CreateRegionCommandMapper>();
            services.AddMapper<ServiceCenter, ServiceCenterResponse, ServiceCenterToResponseDtoMapper>();
            services.AddMapper<CreateServiceCenterCommand, Result<ServiceCenter>, CreateServiceCenterCommandToEntityMapper>();
            services.AddMapper<UpdateServiceCenterCommand, Result<ServiceCenter>, UpdateServiceCenterCommandToEntityMapper>();
            services.AddMapper<Route, RouteResponse, RouteToRouteResponseMapper>();
            services.AddMapper<CreateRouteCommand, Result<Route>, CreateRouteCommandToEntityMapper>();
            services.AddMapper<UpdateRouteCommand, Result<Route>, UpdateRouteCommandToEntityMapper>();
            services.AddMapper<Directory, DirectoryResponse, DirectoryToDirectoryResponseMapper>();
            services.AddMapper<CreateDirectoryCommand, Result<Directory>, CreateDirectoryCommandMapper>();
            services.AddMapper<UpdateDirectoryCommand, Result<Directory>, UpdateDirectoryCommandMapper>();
            services.AddMapper<SubscriptionPlan, SubscriptionPlanResponse, SubscriptionPlanEntityToResponseMapper>();
            services.AddMapper<CreateSubscriptionPlanCommand, Result<SubscriptionPlan>, CreateSubscriptionPlanCommandMapper>();
            services.AddMapper<UpdateSubscriptionPlanCommand, Result<SubscriptionPlan>, UpdateSubscriptionPlanCommandMapper>();
            services.AddMapper<User, CurrentUserResponse, UserToCurrentUserResponseMapper>();

            return services;
        }

        private static IServiceCollection AddMapper<TSource, TDestination, TMapper>(
            this IServiceCollection services)
            where TSource : class
            where TDestination : class
            where TMapper : Mapper<TSource, TDestination>
        {
            services.AddScoped<Mapper<TSource, TDestination>, TMapper>();

            return services;
        }
    }
}
