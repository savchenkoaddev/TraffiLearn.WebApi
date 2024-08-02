using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);

        Task<User?> GetByEmailAsync(Email email);

        Task AddAsync(User user);

        Task DeleteAsync(User user);
    }
}
