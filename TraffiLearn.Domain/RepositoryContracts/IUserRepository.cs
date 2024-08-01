using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);

        Task AddAsync(User user);

        Task DeleteAsync(User user);
    }
}
