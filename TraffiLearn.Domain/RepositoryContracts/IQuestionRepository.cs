using TraffiLearn.Domain.Abstractions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionRepository : IGenericRepository<Question, Guid>
    { }
}
