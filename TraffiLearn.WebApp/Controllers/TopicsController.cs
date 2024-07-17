using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Topics.Queries.GetAll;
using TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic;

namespace TraffiLearn.WebApp.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ISender _sender;

        public TopicsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("/topics")]
        public async Task<IActionResult> AllTopics()
        {
            var topics = await _sender.Send(new GetAllSortedTopicsQuery());

            return View(topics);
        }

        [HttpGet("/topic/{topicId:guid}/questions")]
        public async Task<IActionResult> TopicQuestions(Guid? topicId)
        {
            var questions = await _sender.Send(new GetQuestionsForTopicQuery(topicId));

            return View(questions);
        }
    }
}
