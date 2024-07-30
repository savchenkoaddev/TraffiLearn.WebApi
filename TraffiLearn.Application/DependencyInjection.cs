using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Application.Behaviors;
using TraffiLearn.Application.Queries.Questions;
using TraffiLearn.Application.Queries.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Application.Commands.Tickets.Create;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Application.Queries.Tickets;

namespace TraffiLearn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
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

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly(),
                includeInternalTypes: true);

            return services;
        }
    }
}
