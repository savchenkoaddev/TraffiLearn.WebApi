using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Application.Questions.Commands.AddTopicToQuestion;
using TraffiLearn.Application.Questions.Commands.CreateQuestion;
using TraffiLearn.Application.Questions.Commands.DeleteQuestion;
using TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion;
using TraffiLearn.Application.Questions.Commands.UpdateQuestion;
using TraffiLearn.Application.Questions.Queries.GetAllQuestions;
using TraffiLearn.Application.Questions.Queries.GetQuestionById;
using TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion;

namespace TraffiLearn.WebAPI.Endpoints
{
    public class QuestionsEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/questions");

            group.MapGet("", GetAllQuestions);

            group.MapGet("{questionId:guid}", GetQuestionById);

            group.MapGet("{questionId}/topics", GetTopicsForQuestion);

            group.MapPost("", CreateQuestion);

            group.MapPost("addtopic", AddTopicToQuestion);

            group.MapPut("{questionId:guid}", UpdateQuestion);

            group.MapDelete("{questionId:guid}", DeleteQuestion);

            group.MapDelete("{questionId:guid}/removetopic/{topicId:guid}", RemoveTopicForQuestion);
        }

        #region Commands


        public static async Task<Ok> CreateQuestion(
            QuestionCreateRequest? request,
            ISender sender)
        {
            await sender.Send(new CreateQuestionCommand(request));

            return TypedResults.Ok();
        }

        public static async Task<NoContent> UpdateQuestion(
            Guid? questionId,
            QuestionUpdateRequest? request,
            ISender sender)
        {
            await sender.Send(new UpdateQuestionCommand(questionId, request));

            return TypedResults.NoContent();
        }

        public static async Task<NoContent> DeleteQuestion(
            Guid? questionId,
            ISender sender)
        {
            await sender.Send(new DeleteQuestionCommand(questionId));

            return TypedResults.NoContent();
        }

        public static async Task<Ok> AddTopicToQuestion(
            Guid? questionId,
            Guid? topicId,
            ISender sender)
        {
            await sender.Send(new AddTopicToQuestionCommand(topicId, questionId));

            return TypedResults.Ok();
        }

        public static async Task<Ok> RemoveTopicForQuestion(
            Guid? questionId,
            Guid? topicId,
            ISender sender)
        {
            await sender.Send(new RemoveTopicForQuestionCommand(topicId, questionId));

            return TypedResults.Ok();
        }


        #endregion

        #region Queries


        public static async Task<Ok<IEnumerable<QuestionResponse>>> GetAllQuestions(
            ISender sender)
        {
            var questions = await sender.Send(new GetAllQuestionsQuery());

            return TypedResults.Ok(questions);
        }

        public static async Task<Ok<QuestionResponse>> GetQuestionById(
            Guid? questionId,
            ISender sender)
        {
            var question = await sender.Send(new GetQuestionByIdQuery(questionId));

            return TypedResults.Ok(question);
        }

        public static async Task<Ok<IEnumerable<TopicResponse>>> GetTopicsForQuestion(
            Guid? questionId,
            ISender sender)
        {
            var topics = await sender.Send(new GetTopicsForQuestionQuery(questionId));

            return TypedResults.Ok(topics);
        }


        #endregion
    }
}
