﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.ConfirmChangeEmail
{
    public sealed record ConfirmChangeEmailCommand(
        Guid? UserId,
        string? Token,
        string? NewEmail) : IRequest<Result>;
}
