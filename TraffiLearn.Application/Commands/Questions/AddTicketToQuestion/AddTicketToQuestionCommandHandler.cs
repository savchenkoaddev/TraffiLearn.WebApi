using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddTicketToQuestion
{
    internal sealed class AddTicketToQuestionCommandHandler
        : IRequestHandler<AddTicketToQuestionCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTicketToQuestionCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddTicketToQuestionCommand request, 
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(
                request.TicketId.Value);

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

            var addResult = question.AddTicket(ticket);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
