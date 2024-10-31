using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Delete
{
    internal sealed class DeleteTicketCommandHandler
        : IRequestHandler<DeleteTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTicketCommandHandler(
            ITicketRepository ticketRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken);

            if (ticket is null)
            {
                return TicketErrors.NotFound;
            }

            await _ticketRepository.DeleteAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
