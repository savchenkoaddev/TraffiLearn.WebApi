﻿using MediatR;
using TraffiLearn.Application.DTO.Topics.Request;

namespace TraffiLearn.Application.Topics.Commands.UpdateTopic
{
    public sealed record UpdateTopicCommand(
        Guid TopicId,
        TopicRequest RequestObject) : IRequest;
}
