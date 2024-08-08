using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Ticket, object>>[] includeExpressions);

        Task<IEnumerable<Ticket>> GetAllAsync(
            Expression<Func<Ticket, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Ticket, object>>[] includeExpressions);

        Task<bool> ExistsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Ticket ticket,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Ticket ticket);

        Task DeleteAsync(Ticket ticket);
    }
}
