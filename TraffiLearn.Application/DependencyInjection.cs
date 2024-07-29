using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Questions.Update;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Application.Commands.Topics.Update;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Application.PipelineBehaviors;
using TraffiLearn.Application.Queries.Questions;
using TraffiLearn.Application.Queries.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<Mapper<Question, QuestionResponse>, QuestionToQuestionResponseMapper>();
            services.AddScoped<Mapper<Topic, TopicResponse>, TopicToTopicResponseMapper>();
            services.AddScoped<Mapper<CreateTopicCommand, Result<Topic>>, CreateTopicCommandMapper>();
            services.AddScoped<Mapper<CreateQuestionCommand, Result<Question>>,
                CreateQuestionCommandMapper>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
