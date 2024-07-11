using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface IQuestionService
    {
        Task<QuestionResponse> GetByIdAsync(Guid? questionId);

        Task<IEnumerable<QuestionResponse>> GetAllAsync();

        Task AddAsync(QuestionAddRequest? request);

        Task UpdateAsync(Guid? questionId, QuestionUpdateRequest? request);

        Task DeleteAsync(Guid? questionId);

        Task<QuestionResponse> GetRandomQuestionAsync();

        Task<IEnumerable<QuestionResponse>> GetQuestionsForCategory(Guid? categoryId);

        Task<QuestionResponse> GetRandomQuestionForCategory(Guid? categoryId);

        Task<IEnumerable<QuestionResponse>> GetTheoryTestForCategory(Guid? categoryId);
    }
}
