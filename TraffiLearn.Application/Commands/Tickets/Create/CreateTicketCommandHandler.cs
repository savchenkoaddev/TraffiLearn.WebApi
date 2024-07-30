using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.Create
{
    internal sealed class CreateTicketCommandHandler :
        IRequestHandler<CreateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly Mapper<CreateTicketCommand, Result<Ticket>> _ticketMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketCommandHandler(
            ITicketRepository ticketRepository, 
            Mapper<CreateTicketCommand, Result<Ticket>> ticketMapper, 
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _ticketMapper = ticketMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateTicketCommand request, 
            CancellationToken cancellationToken)
        {
            var mappingResult = _ticketMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var ticket = mappingResult.Value;

            await _ticketRepository.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
