﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.AddQuestionToTicket
{
    internal sealed class AddQuestionToTicketCommandHandler
        : IRequestHandler<AddQuestionToTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionToTicketCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddQuestionToTicketCommand request,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken,
                includeExpressions: ticket => ticket.Questions);

            if (ticket is null)
            {
                return TicketErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.TicketIds);

            if (question is null)
            {
                return TicketErrors.QuestionNotFound;
            }

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

            await _questionRepository.UpdateAsync(question);
            await _ticketRepository.UpdateAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
