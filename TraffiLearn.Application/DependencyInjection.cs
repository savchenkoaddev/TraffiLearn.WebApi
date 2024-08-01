using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Behaviors;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Tickets.Create;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Application.Options;
using TraffiLearn.Application.Queries.Questions;
using TraffiLearn.Application.Queries.Tickets;
using TraffiLearn.Application.Queries.Topics;
using TraffiLearn.Domain.Entities;
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

            services.AddPipelineBehaviors();
            services.AddValidators();
            
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

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<QuestionsSettings>(configuration.GetSection(QuestionsSettings.SectionName));

            return services;
        }
    }
}
