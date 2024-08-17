using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Update
{
    internal sealed class UpdateTicketCommandHandler
        : IRequestHandler<UpdateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdWithQuestionsIdsAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken);

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

            var updateQuestionsResult = await UpdateQuestions(
                ticket,
                request.QuestionsIds,
                cancellationToken);

            if (updateQuestionsResult.IsFailure)
            {
                return updateQuestionsResult.Error;
            }

            await _ticketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> UpdateQuestions(
            Ticket ticket,
            IEnumerable<Guid>? questionsIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var questionId in questionsIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId),
                    cancellationToken);

                if (question is null)
                {
                    return TicketErrors.QuestionNotFound;
                }

                if (!ticket.QuestionsIds.Contains(question.Id))
                {
                    var questionAddResult = ticket.AddQuestion(question.Id);

                    if (questionAddResult.IsFailure)
                    {
                        return questionAddResult.Error;
                    }
                }
            }

            var ticketQuestionsIds = ticket.QuestionsIds.ToList();

            foreach (var questionId in ticketQuestionsIds)
            {
                if (!questionsIds.Contains(questionId.Value))
                {
                    var questionRemoveResult = ticket.RemoveQuestion(questionId);

                    if (questionRemoveResult.IsFailure)
                    {
                        return questionRemoveResult.Error;
                    }
                }
            }

            return Result.Success();
        }
    }
}
