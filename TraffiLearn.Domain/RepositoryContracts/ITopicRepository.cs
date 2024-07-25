using TraffiLearn.Domain.Abstractions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITopicRepository : IGenericRepository<Topic, Guid>
    { }
}
