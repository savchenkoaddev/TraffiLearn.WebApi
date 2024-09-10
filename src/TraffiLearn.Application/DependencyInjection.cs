using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.Auth.Commands.RegisterUser;
using TraffiLearn.Application.Auth.Mappers;
using TraffiLearn.Application.Behaviors;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Application.Comments.Mappers;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.Commands.Update;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Questions.Mappers;
using TraffiLearn.Application.Services;
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
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMappers();

            services.AddMediatR();

            services.AddApplicationServices();

            services.AddPipelineBehaviors();
            services.AddValidators();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly(),
                includeInternalTypes: true);

            return services;
        }

        private static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddScoped<Mapper<Question, QuestionResponse>,
                QuestionToQuestionResponseMapper>();
            services.AddScoped<Mapper<Topic, TopicResponse>, TopicToTopicResponseMapper>();
            services.AddScoped<Mapper<CreateTopicCommand, Result<Topic>>,
                CreateTopicCommandMapper>();
            services.AddScoped<Mapper<CreateQuestionCommand, Result<Question>>,
                CreateQuestionCommandMapper>();
            services.AddScoped<Mapper<CreateTicketCommand, Result<Ticket>>, CreateTicketCommandMapper>();
            services.AddScoped<Mapper<Ticket, TicketResponse>,
                TicketToTicketResponseMapper>();
            services.AddScoped<Mapper<RegisterUserCommand, Result<User>>,
                RegisterUserCommandMapper>();
            services.AddScoped<Mapper<UpdateQuestionCommand, Result<Question>>,
                UpdateQuestionCommandMapper>();
            services.AddScoped<Mapper<UpdateTopicCommand, Result<Topic>>,
                UpdateTopicCommandMapper>();
            services.AddScoped<Mapper<Comment, CommentResponse>,
                CommentToCommentResponseMapper>();
            services.AddScoped<Mapper<RegisterAdminCommand, Result<User>>, RegisterAdminCommandMapper>();
            services.AddScoped<Mapper<User, ApplicationUser>, UserToApplicationUserMapper>();
            services.AddScoped<Mapper<User, UserResponse>, UserToUserResponseMapper>();

            return services;
        }
    }
}
