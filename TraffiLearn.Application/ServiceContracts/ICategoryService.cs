using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface ICategoryService
    {
        Task AddAsync(DrivingCategory item);

        Task DeleteAsync(Guid? key);

        Task UpdateAsync(Guid? key, DrivingCategory? item);

        Task<IEnumerable<DrivingCategory>> GetAllAsync();

        Task<DrivingCategory> GetByIdAsync(Guid? key);

        Task<IEnumerable<Question>> GetQuestionsForCategory(Guid? categoryId);

        Task<Question?> GetRandomQuestionForCategory(Guid? categoryId);

        Task<IEnumerable<Question>> GetTheoryTestForCategory(Guid? categoryId);
    }
}
