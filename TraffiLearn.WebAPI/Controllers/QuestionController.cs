using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.ServiceContracts;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return Ok(await _questionService.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid? id)
        {
            return Ok(await _questionService.GetByIdAsync(id));
        }

        [HttpGet("[action]/{id:guid}")]
        public async Task<IActionResult> Remove(Guid? id)
        {
            await _questionService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RandomOne()
        {
            return Ok(await _questionService.GetRandomQuestionAsync());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(QuestionAddRequest? request)
        {
            await _questionService.AddAsync(request);

            return Ok();
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<IActionResult> Update(Guid? id, QuestionUpdateRequest? request)
        {
            await _questionService.UpdateAsync(id, request);

            return Ok();
        }
    }
}
