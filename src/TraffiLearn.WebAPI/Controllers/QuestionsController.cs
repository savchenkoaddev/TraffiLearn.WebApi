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
using TraffiLearn.Application.Queries.Questions.GetAll;
using TraffiLearn.Application.Queries.Questions.GetById;
using TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest;
using TraffiLearn.Application.Queries.Questions.GetQuestionTickets;
using TraffiLearn.Application.Queries.Questions.GetQuestionTopics;
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


        [HttpGet("")]
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

        [HttpGet("theorytest")]
        public async Task<IActionResult> GetQuestionsForTheoryTest()
        {
            var queryResult = await _sender.Send(new GetQuestionsForTheoryTestQuery());

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

        [HttpPut("{questionId:guid}/addtopic/{topicId:guid}")]
        public async Task<IActionResult> AddTopicToQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new AddTopicToQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/removetopic/{topicId:guid}")]
        public async Task<IActionResult> RemoveTopicFromQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new RemoveTopicFromQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/addticket/{ticketId:guid}")]
        public async Task<IActionResult> AddTicketToQuestion(
            [FromRoute] Guid ticketId,
            [FromRoute] Guid questionId)
        {
            var commandResult = await _sender.Send(new AddTicketToQuestionCommand(
                TicketId: ticketId,
                QuestionId: questionId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{questionId:guid}/removeticket/{ticketId:guid}")]
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

        [HttpPost("addcomment")]
        public async Task<IActionResult> AddComment(AddCommentCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        #endregion
    }
}
