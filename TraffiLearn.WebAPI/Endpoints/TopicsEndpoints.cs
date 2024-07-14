using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Application.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.Topics.Commands.CreateTopic;
using TraffiLearn.Application.Topics.Commands.DeleteTopic;
using TraffiLearn.Application.Topics.Commands.RemoveQuestionForTopic;
using TraffiLearn.Application.Topics.Commands.UpdateTopic;
using TraffiLearn.Application.Topics.Queries.GetAll;
using TraffiLearn.Application.Topics.Queries.GetById;
using TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic;

namespace TraffiLearn.WebAPI.Endpoints
{
    public class TopicsEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/topics");

            group.MapGet("", GetAllTopics);

            group.MapGet("{topicId:guid}", GetTopicById);

            group.MapGet("{topicId:guid}/questions", GetQuestionsForTopic);

            group.MapPost("", CreateTopic);

            group.MapPut("{topicId:guid}/addquestion/{questionId:guid}", AddQuestionToTopic);

            group.MapPut("{topicId:guid}", UpdateTopic);

            group.MapDelete("", DeleteTopic);

            group.MapDelete("{topicId:guid}/removequestion/{questionId:guid}", RemoveQuestionForTopic);
        }

        #region Commands


        public static async Task<Ok> CreateTopic(
            TopicRequest? request,
            ISender sender)
        {
            await sender.Send(new CreateTopicCommand(request));

            return TypedResults.Ok();
        }

        public static async Task<NoContent> UpdateTopic(
            Guid? topicId,
            TopicRequest? request,
            ISender sender)
        {
            await sender.Send(new UpdateTopicCommand(topicId, request));

            return TypedResults.NoContent();
        }

        public static async Task<NoContent> DeleteTopic(
            Guid? topicId,
            ISender sender)
        {
            await sender.Send(new DeleteTopicCommand(topicId));

            return TypedResults.NoContent();
        }


        public static async Task<Ok> AddQuestionToTopic(
            Guid? QuestionId,
            Guid? TopicId,
            ISender sender)
        {
            var command = new AddQuestionToTopicCommand(QuestionId, TopicId);

            await sender.Send(command);

            return TypedResults.Ok();
        }

        public static async Task<NoContent> RemoveQuestionForTopic(
            Guid? QuestionId,
            Guid? TopicId,
            ISender sender)
        {
            var command = new RemoveQuestionForTopicCommand(QuestionId, TopicId);

            await sender.Send(command);

            return TypedResults.NoContent();
        }


        #endregion

        #region Queries


        public static async Task<Ok<IEnumerable<TopicResponse>>> GetAllTopics(
            ISender sender)
        {
            var topics = await sender.Send(new GetAllTopicsQuery());

            return TypedResults.Ok(topics);
        }

        public static async Task<Ok<TopicResponse>> GetTopicById(
            Guid? topicId,
            ISender sender)
        {
            var topic = await sender.Send(new GetTopicByIdQuery(topicId));

            return TypedResults.Ok(topic);
        }

        public static async Task<Ok<IEnumerable<QuestionResponse>>> GetQuestionsForTopic(
            Guid? topicId,
            ISender sender)
        {
            var questions = await sender.Send(new GetQuestionsForTopicQuery(topicId));

            return TypedResults.Ok(questions);
        }


        #endregion
    }
}
