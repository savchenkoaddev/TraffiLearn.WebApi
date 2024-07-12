using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Application.Topics.Commands.CreateTopic;
using TraffiLearn.Application.Topics.Commands.DeleteTopic;
using TraffiLearn.Application.Topics.Commands.UpdateTopic;
using TraffiLearn.Application.Topics.Queries.GetAll;
using TraffiLearn.Application.Topics.Queries.GetById;

namespace TraffiLearn.WebAPI.Endpoints
{
    public class TopicsEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/topics");

            group.MapGet("", GetAllTopics);

            group.MapGet("{topicId:guid}", GetTopicById);

            group.MapPost("", CreateTopic);

            group.MapDelete("", DeleteTopic);

            group.MapPut("{topicId:guid}", UpdateTopic);
        }

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
    }
}
