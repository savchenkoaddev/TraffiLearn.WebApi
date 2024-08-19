using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Questions.Commands.AddCommentToQuestion;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.Commands.Delete;
using TraffiLearn.Application.Questions.Commands.Update;
using TraffiLearn.Application.Questions.Queries.GetAll;
using TraffiLearn.Application.Questions.Queries.GetById;
using TraffiLearn.Application.Questions.Queries.GetQuestionComments;
using TraffiLearn.Application.Questions.Queries.GetQuestionsForTheoryTest;
using TraffiLearn.Application.Questions.Queries.GetQuestionTickets;
using TraffiLearn.Application.Questions.Queries.GetQuestionTopics;
using TraffiLearn.Application.Questions.Queries.GetRandomQuestions;
using TraffiLearn.Application.Users.Commands.DislikeQuestion;
using TraffiLearn.Application.Users.Commands.LikeQuestion;
using TraffiLearn.Application.Users.Commands.MarkQuestion;
using TraffiLearn.Application.Users.Commands.RemoveQuestionDislike;
using TraffiLearn.Application.Users.Commands.RemoveQuestionLike;
using TraffiLearn.Application.Users.Commands.UnmarkQuestion;
using TraffiLearn.Application.Users.Queries.GetCurrentUserDislikedQuestions;
using TraffiLearn.Application.Users.Queries.GetCurrentUserLikedQuestions;
using TraffiLearn.Application.Users.Queries.GetMarkedQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
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

        [HttpGet("questions/random")]
        public async Task<IActionResult> GetRandomQuestion(
            [FromQuery] int amount = 1)
        {
            var queryResult = await _sender.Send(new GetRandomQuestionsQuery(amount));

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

        [HasPermission(Permission.ModifyData)]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion(
            [FromForm] CreateQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(
            [FromForm] UpdateQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new DeleteQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(AddCommentToQuestionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/mark")]
        public async Task<IActionResult> MarkQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new MarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/unmark")]
        public async Task<IActionResult> UnmarkQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new UnmarkQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/like")]
        public async Task<IActionResult> LikeQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new LikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/dislike")]
        public async Task<IActionResult> DislikeQuestion(Guid questionId)
        {
            var commandResult = await _sender.Send(new DislikeQuestionCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/remove-like")]
        public async Task<IActionResult> RemoveQuestionLike(Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionLikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpPut("{questionId:guid}/remove-dislike")]
        public async Task<IActionResult> RemoveQuestionDislike(Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionDislikeCommand(questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
