using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.Auth.Commands.RegisterUser;
using TraffiLearn.Application.Auth.Mappers;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Application.Comments.Mappers;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.Commands.Update;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Questions.Mappers;
using TraffiLearn.Application.Regions.Commands.Create;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Application.Regions.Mappers;
using TraffiLearn.Application.ServiceCenters.Commands.Create;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.Application.ServiceCenters.Mappers;
using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Application.Tickets.Mappers;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.Commands.Update;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Application.Topics.Mappers;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Application.Users.Mappers;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Shared;

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
