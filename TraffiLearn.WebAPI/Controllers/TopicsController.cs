using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.Topics.Commands.CreateTopic;
using TraffiLearn.Application.Topics.Commands.DeleteTopic;
using TraffiLearn.Application.Topics.Commands.RemoveQuestionForTopic;
using TraffiLearn.Application.Topics.Commands.UpdateTopic;
using TraffiLearn.Application.Topics.Queries.GetAllSorted;
using TraffiLearn.Application.Topics.Queries.GetById;
using TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic;

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
        public async Task<IActionResult> GetAllSortedTopics()
        {
            var topics = await _sender.Send(new GetAllSortedTopicsQuery());

            return Ok(topics);
        }

        [HttpGet("{topicId:guid}")]
        public async Task<IActionResult> GetTopicById(Guid topicId)
        {
            var topic = await _sender.Send(new GetTopicByIdQuery(topicId));

            return Ok(topic);
        }

        [HttpGet("{topicId:guid}/questions")]
        public async Task<IActionResult> GetQuestionsForTopic(Guid topicId)
        {
            var questions = await _sender.Send(new GetQuestionsForTopicQuery(topicId));

            return Ok(questions);
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateTopic(TopicRequest request)
        {
            await _sender.Send(new CreateTopicCommand(request));

            return Created();
        }

        [HttpPut("{topicId:guid}")]
        public async Task<IActionResult> UpdateTopic(
            Guid topicId,
            TopicRequest request)
        {
            await _sender.Send(new UpdateTopicCommand(topicId, request));

            return NoContent();
        }

        [HttpPut("{topicId:guid}/addquestion/{questionId:guid}")]
        public async Task<IActionResult> AddQuestionToTopic(
            Guid topicId,
            Guid questionId)
        {
            var command = new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: topicId);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("{topicId:guid}/removequestion/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionForTopic(
            Guid topicId,
            Guid questionId)
        {
            var command = new RemoveQuestionForTopicCommand(
                QuestionId: questionId,
                TopicId: topicId);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{topicId:guid}")]
        public async Task<IActionResult> DeleteTopic(Guid topicId)
        {
            await _sender.Send(new DeleteTopicCommand(topicId));

            return NoContent();
        }


        #endregion
    }
}
