using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Commands.Update
{
    public sealed record UpdateTopicCommand(
        Guid? TopicId,
        int? TopicNumber,
        string? Title,
        IFormFile? Image = null,
        bool RemoveOldImageIfNewMissing = true) : IRequest<Result>;
}
