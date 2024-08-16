using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
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
            var ticket = await _ticketRepository.GetByIdAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken,
                includeExpressions: ticket => ticket.Questions);

            if (ticket is null)
            {
                return QuestionErrors.TicketNotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Tickets);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var ticketRemoveResult = question.RemoveTicket(ticket);

            if (ticketRemoveResult.IsFailure)
            {
                return ticketRemoveResult.Error;
            }

            var questionRemoveResult = ticket.RemoveQuestion(question);

            if (questionRemoveResult.IsFailure)
            {
                return questionRemoveResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _ticketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
