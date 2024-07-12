using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts.Abstractions;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IAnswerRepository : IRepository<Answer, Guid>
    { }
}
