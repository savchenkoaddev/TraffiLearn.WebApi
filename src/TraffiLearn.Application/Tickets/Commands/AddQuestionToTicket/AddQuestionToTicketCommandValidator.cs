﻿using FluentValidation;

namespace TraffiLearn.Application.Tickets.Commands.AddQuestionToTicket
{
    internal sealed class AddQuestionToTicketCommandValidator
        : AbstractValidator<AddQuestionToTicketCommand>
    {
        public AddQuestionToTicketCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.TicketId)
                .NotEmpty();
        }
    }
}
