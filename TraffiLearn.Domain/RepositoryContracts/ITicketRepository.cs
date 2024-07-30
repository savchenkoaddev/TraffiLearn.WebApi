using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(
            Guid ticketId,
            Expression<Func<Ticket, object>> includeExpression = null!);

        Task<IEnumerable<Ticket>> GetAllAsync();

        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(Ticket ticket);

        Task UpdateAsync(Ticket ticket);

        Task DeleteAsync(Ticket ticket);
    }
}
