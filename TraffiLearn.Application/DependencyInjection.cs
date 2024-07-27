using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Application.DTO.Answers.Response;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Response;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Application.PipelineBehaviors;
using TraffiLearn.Application.Questions.Mappers;
using TraffiLearn.Application.Topics.Mappers;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<Mapper<TopicRequest, Topic>, TopicRequestToTopicMapper>();
            services.AddScoped<Mapper<QuestionCreateRequest, Question>, QuestionCreateRequestToQuestionMapper>();
            services.AddScoped<Mapper<Question, QuestionResponse>, QuestionToQuestionResponseMapper>();
            services.AddScoped<Mapper<Topic, TopicResponse>, TopicToTopicResponseMapper>();
            services.AddScoped<Mapper<Answer, AnswerResponse>, AnswerToAnswerResponseMapper>();
            services.AddScoped<Mapper<AnswerRequest, Answer>, AnswerRequestToAnswerMapper>();
            services.AddScoped<Mapper<QuestionTitleDetails, QuestionTitleDetailsResponse>, QtdToQtdResponseMapper>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
