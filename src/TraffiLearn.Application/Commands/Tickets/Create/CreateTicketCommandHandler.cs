using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Tickets.Create
{
    internal sealed class CreateTicketCommandHandler :
        IRequestHandler<CreateTicketCommand, Result>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<CreateTicketCommand, Result<Ticket>> _ticketMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketCommandHandler(
            ITicketRepository ticketRepository, 
            IQuestionRepository questionRepository,
            IUserManagementService userManagementService,
            Mapper<CreateTicketCommand, Result<Ticket>> ticketMapper, 
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _userManagementService = userManagementService;
            _ticketMapper = ticketMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateTicketCommand request, 
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

            var mappingResult = _ticketMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var ticket = mappingResult.Value;

            foreach (var questionId in request.QuestionIds)
            {
                var question = await _questionRepository.GetByIdAsync(
                    questionId: new QuestionId(questionId.Value),
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

                var ticketAddResult = question.AddTicket(ticket);

                if (ticketAddResult.IsFailure)
                {
                    return ticketAddResult.Error;
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
