using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts.Abstractions;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionRepository : IRepository<Question, Guid>
    {
        Task<Question?> GetRandomQuestion();

        Task<Question?> GetRandomQuestionForCategory(Guid categoryId);

        Task<IEnumerable<Question>> GetQuestionsForCategory(Guid categoryId);
    }
}
