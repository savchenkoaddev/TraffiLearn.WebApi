using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.Questions.Commands.CreateQuestion;
using TraffiLearn.Application.Questions.Commands.DeleteQuestion;
using TraffiLearn.Application.Questions.Commands.UpdateQuestion;
using TraffiLearn.Application.Questions.Queries.GetAllQuestions;
using TraffiLearn.Application.Questions.Queries.GetQuestionById;
using TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion;
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

        [HttpGet("/questions/{id:guid}/update")]
        public async Task<IActionResult> UpdateQuestion(
            Guid? id,
            string? returnUrl)
        {
            ViewBag.QuestionId = id;
            ViewBag.AllTopics = await _sender.Send(new GetAllSortedTopicsQuery());
            ViewBag.QuestionTopics = await _sender.Send(new GetTopicsForQuestionQuery(id));
            ViewBag.ReturnUrl = returnUrl;

            var question = await _sender.Send(new GetQuestionByIdQuery(id));

            return View(question);
        }

        [HttpPost("/questions/{id:guid}/update")]
        public async Task<IActionResult> UpdateQuestion(
            [FromRoute] Guid? id,
            [FromForm] QuestionUpdateRequest? question,
            IFormFile? image,
            string? returnUrl)
        {
            await _sender.Send(new UpdateQuestionCommand(id, question, image));

            if (returnUrl is not null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(AllQuestions));
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
