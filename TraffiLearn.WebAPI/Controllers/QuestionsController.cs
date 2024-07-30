using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Questions.AddTopicToQuestion;
using TraffiLearn.Application.Commands.Questions.Create;
using TraffiLearn.Application.Commands.Questions.Delete;
using TraffiLearn.Application.Commands.Questions.RemoveTopicForQuestion;
using TraffiLearn.Application.Commands.Questions.Update;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.Queries.Questions.GetAll;
using TraffiLearn.Application.Queries.Questions.GetById;
using TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion;
using TraffiLearn.Domain.Shared;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
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
        public async Task<IActionResult> GetTopicsForQuestion(
            [FromRoute] Guid questionId)
        {
            var queryResult = await _sender.Send(new GetTopicsForQuestionQuery(questionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateQuestion(
            [FromForm] CreateQuestionCommand command)
        {
            await _sender.Send(command);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(
            [FromForm] UpdateQuestionCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("{questionId:guid}/addtopic/{topicId:guid}")]
        public async Task<IActionResult> AddTopicToQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            await _sender.Send(new AddTopicToQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return NoContent();
        }

        [HttpPut("{questionId:guid}/removetopic/{topicId:guid}")]
        public async Task<IActionResult> RemoveTopicForQuestion(
            [FromRoute] Guid topicId,
            [FromRoute] Guid questionId)
        {
            await _sender.Send(new RemoveTopicForQuestionCommand(
                TopicId: topicId,
                QuestionId: questionId));

            return NoContent();
        }

        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            [FromRoute] Guid questionId)
        {
            await _sender.Send(new DeleteQuestionCommand(questionId));

            return NoContent();
        }


        #endregion
    }
}
