using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts.Abstractions;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IRepository<DrivingCategory, Guid>
    {
        Task<Question?> GetRandomQuestionForCategory(Guid categoryId);

        Task<IEnumerable<Question>> GetQuestionsForCategory(Guid categoryId);

        Task<IEnumerable<Question>> GetTheoryTestForCategory(Guid categoryId);
    }
}
