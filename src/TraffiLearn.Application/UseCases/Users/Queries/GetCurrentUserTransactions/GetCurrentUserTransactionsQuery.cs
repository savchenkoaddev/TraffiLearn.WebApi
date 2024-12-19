using MediatR;
using TraffiLearn.Application.UseCases.Transactions.DTO;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserTransactions
{
    public sealed record GetCurrentUserTransactionsQuery 
        : IRequest<Result<IEnumerable<TransactionResponse>>>;
}
