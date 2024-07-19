using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.Questions.Commands.CreateQuestion;
using TraffiLearn.Application.Questions.Commands.DeleteQuestion;
using TraffiLearn.Application.Questions.Queries.GetAllQuestions;
using TraffiLearn.Application.Topics.Queries.GetAll;
using TraffiLearn.WebApp.Helpers;
using TraffiLearn.WebApp.Options;

namespace TraffiLearn.WebApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ISender _sender;
        private readonly PaginationSettings _paginationSettings;

        public QuestionsController(ISender sender, IOptions<PaginationSettings> paginationSettings)
        {
            _sender = sender;
            _paginationSettings = paginationSettings.Value;
        }

        [HttpGet("/questions/add")]
        public async Task<IActionResult> AddQuestion()
        {
            var topics = await _sender.Send(new GetAllSortedTopicsQuery());

            return View(topics);
        }

        [HttpPost("/questions/add")]
        public async Task<IActionResult> AddQuestion(
            [FromForm] QuestionCreateRequest? request,
            [FromForm] IFormFile? image)
        {
            await _sender.Send(new CreateQuestionCommand(request, image));

            return RedirectToAction(nameof(AddQuestion));
        }

        [HttpGet("/questions")]
        public async Task<IActionResult> AllQuestions(int page = 1)
        {
            var questions = await _sender.Send(new GetAllQuestionsQuery());

            var paginationHelper = new PaginationHelper<QuestionResponse>(questions.Count(), _paginationSettings.PageSize.Value);
            var paginatedQuestions = paginationHelper.GetPaginatedData(questions, page);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = paginationHelper.TotalPages;

            return View(paginatedQuestions);
        }

        [HttpDelete("/questions/{id:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            Guid? id)
        {
            await _sender.Send(new DeleteQuestionCommand(id));

            return NoContent();
        }
    }
}
