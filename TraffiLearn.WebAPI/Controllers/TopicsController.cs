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
        public async Task<IActionResult> GetTopicById(GetTopicByIdQuery query)
        {
            return Ok(await _sender.Send(query));
        }

        [HttpGet("{topicId:guid}/questions")]
        public async Task<IActionResult> GetQuestionsForTopic(GetQuestionsForTopicQuery query)
        {
            return Ok(await _sender.Send(query));
        }


        #endregion

        #region Commands


        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicCommand command)
        {
            await _sender.Send(command);

            return Created();
        }

        [HttpPut("{topicId:guid}")]
        public async Task<IActionResult> UpdateTopic(UpdateTopicCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("{topicId:guid}/addquestion/{questionId:guid}")]
        public async Task<IActionResult> AddQuestionToTopic(AddQuestionToTopicCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("{topicId:guid}/removequestion/{questionId:guid}")]
        public async Task<IActionResult> RemoveQuestionForTopic(RemoveQuestionForTopicCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{topicId:guid}")]
        public async Task<IActionResult> DeleteTopic(DeleteTopicCommand command)
        {
            await _sender.Send(command);

            return NoContent();
        }


        #endregion
    }
}
