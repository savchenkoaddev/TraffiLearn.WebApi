﻿using FluentValidation;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionTickets
{
    internal sealed class GetQuestionTicketsQueryValidator
        : AbstractValidator<GetQuestionTicketsQuery>
    {
        public GetQuestionTicketsQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
