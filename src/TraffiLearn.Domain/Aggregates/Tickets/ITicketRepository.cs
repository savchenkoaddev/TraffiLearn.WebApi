using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Tickets
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

        Task<Ticket?> GetRandomRecordAsync(
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
