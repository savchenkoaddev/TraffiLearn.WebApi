using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Commands.Create
{
    public sealed record CreateTopicCommand(
        int? TopicNumber,
        string? Title,
        IFormFile? Image = null) : IRequest<Result<Guid>>;
}
