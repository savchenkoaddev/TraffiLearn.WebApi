using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdRawAsync(Guid ticketId);

        Task<Ticket?> GetByIdWithQuestionsAsync(Guid ticketId);

        Task<IEnumerable<Ticket>> GetAllAsync();

        Task<bool> ExistsAsync(Guid ticketId);

        Task AddAsync(Ticket ticket);

        Task UpdateAsync(Ticket ticket);

        Task DeleteAsync(Ticket ticket);
    }
}
