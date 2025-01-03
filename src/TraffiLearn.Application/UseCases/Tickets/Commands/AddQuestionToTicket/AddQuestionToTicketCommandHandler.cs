﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.AddQuestionToTicket
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
                ticketId: new TicketId(request.TicketId),
                cancellationToken);

            if (ticket is null)
            {
                return TicketErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId),
                cancellationToken);

            if (question is null)
            {
                return TicketErrors.QuestionNotFound;
            }

            var questionAddResult = ticket.AddQuestion(question);

            if (questionAddResult.IsFailure)
            {
                return questionAddResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
