using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.ServiceContracts;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid? id)
        {
            return Ok(await _categoryService.GetByIdAsync(id));
        }

        [HttpGet("[action]/{id:guid}")]
        public async Task<IActionResult> Remove(Guid? id)
        {
            await _categoryService.DeleteAsync(id);

            return NoContent();
        }

        //[HttpGet("{id:guid}/[action]")]
        //public async Task<IActionResult> Questions(Guid? id)
        //{
        //    return Ok(await _categoryService.GetQuestionsForCategory(id));
        //}

        //[HttpGet("{id:guid}/[action]")]
        //public async Task<IActionResult> RandomQuestion(Guid? id)
        //{
        //    return Ok(await _categoryService.GetRandomQuestionForCategory(id));
        //}

        //[HttpGet("{id:guid}/[action]")]
        //public async Task<IActionResult> TheoryTest(Guid? id)
        //{
        //    return Ok(await _categoryService.GetTheoryTestForCategory(id));
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CategoryRequest? request)
        {
            await _categoryService.AddAsync(request);

            return Ok();
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<IActionResult> Update(Guid? id, CategoryRequest? request)
        {
            await _categoryService.UpdateAsync(id, request);

            return Ok();
        }
    }
}
