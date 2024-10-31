﻿using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetTicketQuestions
{
    public sealed record GetTicketQuestionsQuery(
        Guid? TicketId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
