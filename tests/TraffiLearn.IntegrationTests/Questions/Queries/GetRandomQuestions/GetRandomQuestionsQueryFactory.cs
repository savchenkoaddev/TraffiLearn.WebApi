using TraffiLearn.Application.UseCases.Questions.Queries.GetRandomQuestions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetRandomQuestions
{
    public sealed class GetRandomQuestionsQueryFactory
    {
        public IEnumerable<GetRandomQuestionsQuery> CreateInvalidQueries()
        {
            return [
                new GetRandomQuestionsQuery(
                    Amount: null),

                new GetRandomQuestionsQuery(
                    Amount: -1),

                new GetRandomQuestionsQuery(
                    Amount: 0)
            ];
        }
    }
}
