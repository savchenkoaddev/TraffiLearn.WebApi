﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
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
            var ticket = await _ticketRepository.GetByIdWithQuestionsAsync(
                request.TicketId.Value);

            if (ticket is null)
            {
                return TicketErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdWithTicketsAsync(request.QuestionId.Value);

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
