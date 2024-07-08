using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface IQuestionsService
    {
        Task<QuestionResponse> GetByIdAsync(Guid? questionId);

        Task<IEnumerable<QuestionResponse>> GetAllAsync();

        Task<bool> ExistsAsync(Guid? questionId);

        Task AddAsync(QuestionAddRequest? item);

        Task UpdateAsync(Guid? questionId, QuestionUpdateRequest? item);

        Task DeleteAsync(QuestionDeleteRequest? request);

        Task<QuestionResponse> GetRandomQuestionAsync();
    }
}
