﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Update
{
    public sealed record UpdateTicketCommand(
        Guid? TicketId,
        int? TicketNumber,
        List<Guid>? QuestionIds) : IRequest<Result>;
}
