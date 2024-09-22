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

namespace TraffiLearn.Application.Extensions.DI
{
    internal static class MapperExtensions
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
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
