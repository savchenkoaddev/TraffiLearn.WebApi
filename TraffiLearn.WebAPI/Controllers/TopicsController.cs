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
            var topics = await _sender.Send(new GetAllSortedTopicsByNumberQuery());

            return Ok(topics);
        }

        [HttpGet("{topicId:guid}")]
        public async Task<IActionResult> GetTopicById(
            [FromRoute] Guid topicId)
        {
            return Ok(await _sender.Send(new GetTopicByIdQuery(topicId)));
        }

        [HttpGet("{topicId:guid}/questions")]
        public async Task<IActionResult> GetQuestionsForTopic(
            [FromRoute] Guid topicId)
        {
            return Ok(await _sender.Send(new GetQuestionsForTopicQuery(topicId)));
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicCommand command)
        {
            await _sender.Send(command);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic(UpdateTopicCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("{topicId:guid}/addquestion/{questionId:guid}")]
        public async Task<IActionResult> AddQuestionToTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            await _sender.Send(new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return NoContent();
        }

        [HttpPut("{topicId:guid}/removequestion/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionForTopic(
            [FromRoute] Guid questionId,
            [FromRoute] Guid topicId)
        {
            await _sender.Send(new RemoveQuestionForTopicCommand(
                QuestionId: questionId,
                TopicId: topicId));

            return NoContent();
        }

        [HttpDelete("{topicId:guid}")]
        public async Task<IActionResult> DeleteTopic(
            [FromRoute] Guid topicId)
        {
            await _sender.Send(new DeleteTopicCommand(topicId));

            return NoContent();
        }


        #endregion
    }
}
