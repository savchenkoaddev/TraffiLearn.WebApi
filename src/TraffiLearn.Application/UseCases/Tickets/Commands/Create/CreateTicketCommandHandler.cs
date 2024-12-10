using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.Create
{
    internal sealed class CreateTicketCommandHandler :
        IRequestHandler<CreateTicketCommand, Result<Guid>>
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

        public async Task<Result<Guid>> Handle(
            CreateTicketCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _ticketMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var ticket = mappingResult.Value;

            foreach (var questionId in request.QuestionIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId),
                    cancellationToken);

                if (question is null)
                {
                    return Result.Failure<Guid>(TicketErrors.QuestionNotFound);
                }

                var questionAddResult = ticket.AddQuestion(question);

                if (questionAddResult.IsFailure)
                {
                    return Result.Failure<Guid>(questionAddResult.Error);
                }
            }

            await _ticketRepository.InsertAsync(
                ticket,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(ticket.Id.Value);
        }
    }
}
