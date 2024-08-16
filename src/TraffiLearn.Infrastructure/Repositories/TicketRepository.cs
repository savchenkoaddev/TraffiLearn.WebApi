using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            Ticket ticket,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Tickets.AddAsync(
                ticket,
                cancellationToken);
        }

        public Task DeleteAsync(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default)
        {
            return (await _dbContext.Tickets.FindAsync(
                keyValues: [ticketId],
                cancellationToken)) is not null;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(
            Expression<Func<Ticket, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Ticket, object>>[] includeExpressions)
        {
            IQueryable<Ticket> tickets = _dbContext.Tickets;

            foreach (var includeExpression in includeExpressions)
            {
                tickets = tickets.Include(includeExpression);
            }

            if (orderByExpression is not null)
            {
                tickets = tickets.OrderBy(orderByExpression);
            }

            return await tickets
                .ToListAsync(cancellationToken);
        }

        public async Task<Ticket?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<Ticket, object>>[] includeExpressions)
        {
            var sql = """
                SELECT TOP 1 *
                FROM {0}
                ORDER BY NEWID()
            """;

            var formattedSql = string.Format(
               sql,
               nameof(ApplicationDbContext.Tickets));

            var query = _dbContext.Tickets
                .FromSqlRaw(formattedSql);

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Ticket?> GetByIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Ticket, object>>[] includeExpressions)
        {
            var query = _dbContext.Tickets.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(
                    c => c.Id == ticketId,
                    cancellationToken);
        }

        public Task UpdateAsync(Ticket ticket)
        {
            _dbContext.Tickets.Update(ticket);

            return Task.CompletedTask;
        }
    }
}
