using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.Questions.Commands.AddTopicToQuestion;
using TraffiLearn.Application.Questions.Commands.CreateQuestion;
using TraffiLearn.Application.Questions.Commands.DeleteQuestion;
using TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion;
using TraffiLearn.Application.Questions.Commands.UpdateQuestion;
using TraffiLearn.Application.Questions.Queries.GetAllQuestions;
using TraffiLearn.Application.Questions.Queries.GetQuestionById;
using TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion;

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
            var questions = await _sender.Send(new GetAllQuestionsQuery());

            return Ok(questions);
        }

        [HttpGet("{questionId:guid}")]
        public async Task<IActionResult> GetQuestionById(Guid? questionId)
        {
            var question = await _sender.Send(new GetQuestionByIdQuery(questionId));

            return Ok(question);
        }

        [HttpGet("{questionId:guid}/topics")]
        public async Task<IActionResult> GetTopicsForQuestion(Guid? questionId)
        {
            var topics = await _sender.Send(new GetTopicsForQuestionQuery(questionId));

            return Ok(topics);
        }


        #endregion

        #region Commands

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(
            [FromForm] QuestionCreateRequest? request,
            [FromForm] IFormFile? image)
        {
            await _sender.Send(new CreateQuestionCommand(request, image));

            return Created();
        }

        [HttpPut("{questionId:guid}")]
        public async Task<IActionResult> UpdateQuestion(
            Guid? questionId,
            [FromForm] QuestionUpdateRequest? request,
            [FromForm] IFormFile? image)
        {
            await _sender.Send(new UpdateQuestionCommand(questionId, request, image));

            return NoContent();
        }

        [HttpPut("{questionId:guid}/addtopic/{topicId:guid}")]
        public async Task<IActionResult> AddTopicToQuestion(
            Guid? questionId,
            Guid? topicId)
        {
            await _sender.Send(new AddTopicToQuestionCommand(topicId, questionId));

            return NoContent();
        }

        [HttpPut("{questionId:guid}/removetopic/{topicId:guid}")]
        public async Task<IActionResult> RemoveTopicForQuestion(
            Guid? questionId,
            Guid? topicId,
            ISender sender)
        {
            await sender.Send(new RemoveTopicForQuestionCommand(topicId, questionId));

            return NoContent();
        }

        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            Guid? questionId)
        {
            await _sender.Send(new DeleteQuestionCommand(questionId));

            return NoContent();
        }

        #endregion
    }
}
