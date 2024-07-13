using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts.Abstractions;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITopicRepository : IRepository<Topic, Guid>
    { }
}
