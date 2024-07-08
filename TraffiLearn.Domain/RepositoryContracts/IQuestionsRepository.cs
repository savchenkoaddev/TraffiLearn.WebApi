using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts.Abstractions;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionsRepository : IRepository<Question, Guid>
    {
        Task<Question?> GetRandomQuestionAsync();
    }
}
