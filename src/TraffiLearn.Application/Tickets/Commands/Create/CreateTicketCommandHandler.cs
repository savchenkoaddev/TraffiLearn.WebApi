using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Create
{
    internal sealed class CreateTicketCommandHandler :
        IRequestHandler<CreateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<CreateTicketCommand, Result<Ticket>> _ticketMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            Mapper<CreateTicketCommand, Result<Ticket>> ticketMapper,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
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

            foreach (var questionId in request.QuestionIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId),
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
            }

            await _ticketRepository.AddAsync(
                ticket,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
