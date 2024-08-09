using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.Application.Commands.Tickets.Update
{
    internal sealed class UpdateTicketCommandHandler
        : IRequestHandler<UpdateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTicketCommand request,
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
                cancellationToken,
                includeExpressions: ticket => ticket.Questions);

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
            IEnumerable<Guid?>? questionsIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var questionId in questionsIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId.Value),
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

                    var ticketAddResult = question.AddTicket(ticket);

                    if (ticketAddResult.IsFailure)
                    {
                        return ticketAddResult.Error;
                    }
                }
            }

            var ticketQuestions = ticket.Questions.ToList();

            foreach (var question in ticketQuestions)
            {
                if (!questionsIds.Contains(question.Id.Value))
                {
                    var questionRemoveResult = ticket.RemoveQuestion(question);

                    if (questionRemoveResult.IsFailure)
                    {
                        return questionRemoveResult.Error;
                    }

                    var ticketRemoveResult = question.RemoveTicket(ticket);

                    if (ticketRemoveResult.IsFailure)
                    {
                        return ticketRemoveResult.Error;
                    }
                }
            }

            return Result.Success();
        }
    }
}
