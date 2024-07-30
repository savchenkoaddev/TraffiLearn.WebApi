using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.RemoveTicketFromQuestion
{
    internal sealed class RemoveTicketFromQuestionCommandHandler : IRequestHandler<RemoveTicketFromQuestionCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTicketFromQuestionCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository, 
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveTicketFromQuestionCommand request, 
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId.Value);

            if (ticket is null)
            {
                return QuestionErrors.TicketNotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: question => question.Tickets);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var removeResult = question.RemoveTicket(ticket);

            if (removeResult.IsFailure)
            {
                return removeResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
