using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Behaviors;
using TraffiLearn.Application.Commands.Auth.RegisterAdmin;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Questions.Update;
using TraffiLearn.Application.Commands.Tickets.Create;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Application.Commands.Topics.Update;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Application.Identity;
using TraffiLearn.Application.Mappers.Auth;
using TraffiLearn.Application.Mappers.Comments;
using TraffiLearn.Application.Mappers.Questions;
using TraffiLearn.Application.Mappers.Tickets;
using TraffiLearn.Application.Mappers.Topics;
using TraffiLearn.Application.Options;
using TraffiLearn.Application.Services;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
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
            services.AddOptions(configuration);

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
            services.AddScoped<Mapper<Topic, TopicWithQuestionsResponse>,
                TopicToTopicWithQuestionsResponseMapper>();
            services.AddScoped<Mapper<Ticket, TicketWithQuestionsResponse>,
                TicketToTicketWithQuestionsResponseMapper>();
            services.AddScoped<Mapper<Topic, TopicResponse>, TopicToTopicResponseMapper>();
            services.AddScoped<Mapper<CreateTopicCommand, Result<Topic>>,
                CreateTopicCommandMapper>();
            services.AddScoped<Mapper<CreateQuestionCommand, Result<Question>>,
                CreateQuestionCommandMapper>();
            services.AddScoped<Mapper<CreateTicketCommand, Result<Ticket>>, CreateTicketCommandMapper>();
            services.AddScoped<Mapper<Ticket, TicketResponse>,
                TicketToTicketResponseMapper>();
            services.AddScoped<Mapper<RegisterUserCommand, Result<UserId>>,
                RegisterUserCommandMapper>();
            services.AddScoped<Mapper<UpdateQuestionCommand, Result<Question>>,
                UpdateQuestionCommandMapper>();
            services.AddScoped<Mapper<UpdateTopicCommand, Result<Topic>>,
                UpdateTopicCommandMapper>();
            services.AddScoped<Mapper<Comment, CommentResponse>,
                CommentToCommentResponseMapper>();
            services.AddScoped<Mapper<RegisterAdminCommand, Result<UserId>>, RegisterAdminCommandMapper>();
            services.AddScoped<Mapper<UserId, ApplicationUser>, UserToApplicationUserMapper>();

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<QuestionsSettings>(
                configuration.GetSection(QuestionsSettings.SectionName));
            services.Configure<LoginSettings>(
                configuration.GetSection(LoginSettings.SectionName));

            return services;
        }
    }
}
