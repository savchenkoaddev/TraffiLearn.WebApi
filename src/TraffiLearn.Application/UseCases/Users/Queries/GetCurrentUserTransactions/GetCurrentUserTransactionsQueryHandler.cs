using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.Transactions.DTO;
using TraffiLearn.Domain.Transactions;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserTransactions
{
    internal sealed class GetCurrentUserTransactionsQueryHandler
        : IRequestHandler<GetCurrentUserTransactionsQuery, Result<IEnumerable<TransactionResponse>>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<Transaction, TransactionResponse> _mapper;

        public GetCurrentUserTransactionsQueryHandler(
            ITransactionRepository transactionRepository,
            IUserContextService<Guid> userContextService,
            Mapper<Transaction, TransactionResponse> mapper)
        {
            _transactionRepository = transactionRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TransactionResponse>>> Handle(
            GetCurrentUserTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUserId = new UserId(_userContextService.GetAuthenticatedUserId());

            var transactions = await _transactionRepository.GetAllByUserId(
                userId: currentUserId,
                cancellationToken);

            return Result.Success(_mapper.Map(transactions));
        }
    }
}
