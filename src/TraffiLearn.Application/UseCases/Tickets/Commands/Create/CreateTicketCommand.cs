﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.Create
{
    public sealed record CreateTicketCommand(
        int TicketNumber,
        List<Guid> QuestionIds) : IRequest<Result<Guid>>;
}
