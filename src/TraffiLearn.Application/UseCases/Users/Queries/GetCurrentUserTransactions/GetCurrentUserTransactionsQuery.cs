using MediatR;
using TraffiLearn.Application.UseCases.Transactions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserTransactions
{
    public sealed record GetCurrentUserTransactionsQuery 
        : IRequest<Result<IEnumerable<TransactionResponse>>>;
}
