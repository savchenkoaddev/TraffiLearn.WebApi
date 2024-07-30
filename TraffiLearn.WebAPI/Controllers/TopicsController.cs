using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Topics.AddQuestionToTopic;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Application.Commands.Topics.Delete;
using TraffiLearn.Application.Commands.Topics.RemoveQuestionForTopic;
using TraffiLearn.Application.Commands.Topics.Update;
using TraffiLearn.Application.Queries.Topics.GetAllSortedByNumber;
using TraffiLearn.Application.Queries.Topics.GetById;
using TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ISender _sender;

        public TopicsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        [HttpGet]
        public async Task<IActionResult> GetAllSortedTopicsByNumber()
        {
            var queryResult = await _sender.Send(new GetAllSortedTopicsByNumberQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{topicId:guid}")]
        public async Task<IActionResult> GetTopicById(
            [FromRoute] Guid topicId)
        {
            var queryResult = await _sender.Send(new GetTopicByIdQuery(topicId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("{topicId:guid}/questions")]
        public async Task<IActionResult> GetQuestionsForTopic(
            [FromRoute] Guid topicId)
        {
            var queryResult = await _sender.Send(new GetQuestionsForTopicQuery(topicId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic(UpdateTopicCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{topicId:guid}/addquestion/{questionId:guid}")]
        public async Task<IActionResult> AddQuestionToTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{topicId:guid}/removequestion/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionForTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new RemoveQuestionForTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpDelete("{topicId:guid}")]
        public async Task<IActionResult> DeleteTopic(
            [FromRoute] Guid topicId)
        {
            var commandResult = await _sender.Send(new DeleteTopicCommand(topicId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
