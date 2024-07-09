using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.DTO.Categories.Response;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryRequest? item);

        Task DeleteAsync(Guid? key);

        Task UpdateAsync(Guid? key, CategoryRequest? item);

        Task<IEnumerable<CategoryResponse>> GetAllAsync();

        Task<CategoryResponse> GetByIdAsync(Guid? key);

        Task<IEnumerable<QuestionResponse>> GetQuestionsForCategory(Guid? categoryId);

        Task<QuestionResponse?> GetRandomQuestionForCategory(Guid? categoryId);

        Task<IEnumerable<QuestionResponse>> GetTheoryTestForCategory(Guid? categoryId);
    }
}
