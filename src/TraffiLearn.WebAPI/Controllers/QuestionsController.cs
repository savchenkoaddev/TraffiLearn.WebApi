using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Questions.AddComment;
using TraffiLearn.Application.Commands.Questions.AddTicketToQuestion;
using TraffiLearn.Application.Commands.Questions.AddTopicToQuestion;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Questions.Delete;
using TraffiLearn.Application.Commands.Questions.RemoveTicketFromQuestion;
using TraffiLearn.Application.Commands.Questions.RemoveTopicFromQuestion;
using TraffiLearn.Application.Commands.Questions.Update;
using TraffiLearn.Application.Commands.Users.DislikeQuestion;
using TraffiLearn.Application.Commands.Users.LikeQuestion;
using TraffiLearn.Application.Commands.Users.MarkQuestion;
using TraffiLearn.Application.Commands.Users.RemoveQuestionDislike;
using TraffiLearn.Application.Commands.Users.RemoveQuestionLike;
using TraffiLearn.Application.Commands.Users.UnmarkQuestion;
using TraffiLearn.Application.Queries.Questions.GetAll;
using TraffiLearn.Application.Queries.Questions.GetById;
using TraffiLearn.Application.Queries.Questions.GetQuestionComments;
using TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest;
using TraffiLearn.Application.Queries.Questions.GetQuestionTickets;
using TraffiLearn.Application.Queries.Questions.GetQuestionTopics;
using TraffiLearn.Application.Queries.Users.GetCurrentUserDislikedQuestions;
using TraffiLearn.Application.Queries.Users.GetCurrentUserLikedQuestions;
using TraffiLearn.Application.Queries.Users.GetMarkedQuestions;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Authorize]
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ISender _sender;

        public QuestionsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var queryResult = await _sender.Send(new GetAllQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{questionId:guid}")]
        public async Task<IActionResult> GetQuestionById(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionByIdQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{questionId:guid}/topics")]
        public async Task<IActionResult> GetQuestionTopics(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionTopicsQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{questionId:guid}/tickets")]
        public async Task<IActionResult> GetQuestionTickets(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetQuestionTicketsQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("theory-test")]
        public async Task<IActionResult> GetQuestionsForTheoryTest()
        {
            var queryResult = await _sender.Send(new GetQuestionsForTheoryTestQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{questionId:guid}/comments")]
        public async Task<IActionResult> GetQuestionComments(Guid questionId)
        {
            var queryResult = await _sender.Send(
                new GetQuestionCommentsQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("marked")]
        public async Task<IActionResult> GetMarkedQuestions()
        {
            var queryResult = await _sender.Send(new GetMarkedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("liked")]
        public async Task<IActionResult> GetLikedQuestions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserLikedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("disliked")]
        public async Task<IActionResult> GetDislikedQuestions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserDislikedQuestionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateQuestion(
            [FromForm] CreateQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(
            [FromForm] UpdateQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/add-topic/{topicId:guid}")]
        public async Task<IActionResult> AddTopicToQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new AddTopicToQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/remove-topic/{topicId:guid}")]
        public async Task<IActionResult> RemoveTopicFromQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveTopicFromQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/add-ticket/{ticketId:guid}")]
        public async Task<IActionResult> AddTicketToQuestion(
            [FromRoute] Guid ticketId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new AddTicketToQuestionCommand(
                TicketId: ticketId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/remove-ticket/{ticketId:guid}")]
        public async Task<IActionResult> RemoveTicketFromQuestion(
            [FromRoute] Guid ticketId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveTicketFromQuestionCommand(
                TicketId: ticketId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new DeleteQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(AddCommentCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/mark")]
        public async Task<IActionResult> MarkQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new MarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/unmark")]
        public async Task<IActionResult> UnmarkQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new UnmarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/like")]
        public async Task<IActionResult> LikeQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new LikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/dislike")]
        public async Task<IActionResult> DislikeQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new DislikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/remove-like")]
        public async Task<IActionResult> RemoveQuestionLike(Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionLikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/remove-dislike")]
        public async Task<IActionResult> RemoveQuestionDislike(Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionDislikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
