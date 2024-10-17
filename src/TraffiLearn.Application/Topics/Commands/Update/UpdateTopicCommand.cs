﻿using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Update
{
    public sealed record UpdateTopicCommand(
        Guid? TopicId,
        int? TopicNumber,
        string? Title,
        IFormFile? Image,
        bool RemoveOldImageIfNewNotProvided = true) : IRequest<Result>;
}
