using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Questions.Queries.GetAllQuestions;

namespace TraffiLearn.WebApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ISender _sender;

        public QuestionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("/questions")]
        public async Task<IActionResult> AllQuestions()
        {
            var questions = await _sender.Send(new GetAllQuestionsQuery());

            return View(questions);
        }
    }
}
