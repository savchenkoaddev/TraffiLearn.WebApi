using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.Questions.Queries.GetAllQuestions
{
    public sealed record GetAllQuestionsQuery : IRequest<IEnumerable<QuestionResponse>>;
}
