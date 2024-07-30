using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Tickets.Update
{
    internal sealed class UpdateTicketCommandHandler
        : IRequestHandler<UpdateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository, 
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTicketCommand request, 
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId.Value);

            if (ticket is null)
            {
                return TicketErrors.NotFound;
            }

            var numberCreateResult = TicketNumber.Create(request.TicketNumber.Value);

            if (numberCreateResult.IsFailure)
            {
                return numberCreateResult.Error;
            }

            var updateResult = ticket.Update(numberCreateResult.Value);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _ticketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
