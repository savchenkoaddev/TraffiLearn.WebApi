using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.Application.Commands.Tickets.Delete
{
    internal sealed class DeleteTicketCommandHandler
        : IRequestHandler<DeleteTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTicketCommandHandler(
            ITicketRepository ticketRepository, 
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        public async Task<Result> Handle(
            DeleteTicketCommand request, 
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

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
