using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Tickets.TicketNumbers;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.Update
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
            var ticket = await _ticketRepository.GetByIdWithQuestionsAsync(
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
                request.QuestionIds,
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
            IEnumerable<Guid>? questionIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var questionId in questionIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId),
                    cancellationToken);

                if (question is null)
                {
                    return TicketErrors.QuestionNotFound;
                }

                if (!ticket.Questions.Contains(question))
                {
                    var questionAddResult = ticket.AddQuestion(question);

                    if (questionAddResult.IsFailure)
                    {
                        return questionAddResult.Error;
                    }
                }
            }

            var ticketQuestions = ticket.Questions.ToList();

            foreach (var question in ticketQuestions)
            {
                if (!questionIds.Contains(question.Id.Value))
                {
                    var questionRemoveResult = ticket.RemoveQuestion(question);

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
