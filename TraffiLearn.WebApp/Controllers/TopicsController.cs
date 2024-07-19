using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.Topics.Commands.CreateTopic;
using TraffiLearn.Application.Topics.Commands.DeleteTopic;
using TraffiLearn.Application.Topics.Queries.GetAll;
using TraffiLearn.Application.Topics.Queries.GetById;
using TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic;
using TraffiLearn.WebApp.Helpers;
using TraffiLearn.WebApp.Options;

namespace TraffiLearn.WebApp.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ISender _sender;
        private readonly PaginationSettings _paginationSettings;

        public TopicsController(
            ISender sender,
            IOptions<PaginationSettings> paginationSettings)
        {
            _sender = sender;
            _paginationSettings = paginationSettings.Value;
        }

        [HttpGet("/topics")]
        public async Task<IActionResult> AllTopics()
        {
            var topics = await _sender.Send(new GetAllSortedTopicsQuery());

            return View(topics);
        }

        [HttpGet("/topic/{topicId:guid}/questions")]
        public async Task<IActionResult> TopicQuestions(Guid? topicId, int page = 1)
        {
            var questions = await _sender.Send(new GetQuestionsForTopicQuery(topicId.Value));

            var paginationHelper = new PaginationHelper<QuestionResponse>(questions.Count(), _paginationSettings.PageSize.Value);
            var paginatedQuestions = paginationHelper.GetPaginatedData(questions, page);

            ViewBag.TopicTitle = (await _sender.Send(new GetTopicByIdQuery(topicId.Value))).Title;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = paginationHelper.TotalPages;
            ViewBag.TopicId = topicId;

            return View(paginatedQuestions);
        }

        [HttpGet("/topics/add")]
        public IActionResult AddTopic()
        {
            return View();
        }

        [HttpPost("/topics/add")]
        public async Task<IActionResult> AddTopic(
            TopicRequest? topicRequest)
        {
            await _sender.Send(new CreateTopicCommand(topicRequest));

            return RedirectToAction(nameof(AddTopic));
        }

        [HttpDelete("/topics/{id:guid}")]
        public async Task<IActionResult> DeleteTopic(
           Guid? id)
        {
            await _sender.Send(new DeleteTopicCommand(id));

            return NoContent();
        }
    }
}
