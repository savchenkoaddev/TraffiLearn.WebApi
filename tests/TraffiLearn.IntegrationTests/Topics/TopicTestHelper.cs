using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Application.Topics.Queries.GetAllSortedByNumber;
using TraffiLearn.Application.Topics.Queries.GetTopicQuestions;

namespace TraffiLearn.IntegrationTests.Topics
{
    internal sealed class TopicTestHelper
    {
        private readonly ISender _sender;

        public TopicTestHelper(ISender sender)
        {
            _sender = sender;
        }

        public async Task CreateTopicAsync(int number = 1, string title = "Value")
        {
            await _sender.Send(new CreateTopicCommand(number, title));
        }

        public async Task<Guid> GetFirstTopicIdAsync()
        {
            return (await GetAllTopicsAsync()).First().TopicId;
        }

        public async Task<IEnumerable<TopicResponse>> GetAllTopicsAsync()
        {
            return (await _sender.Send(new GetAllSortedTopicsByNumberQuery())).Value;
        }

        public async Task<IEnumerable<QuestionResponse>> GetTopicQuestionsAsync(Guid topicId)
        {
            return (await _sender.Send(new GetTopicQuestionsQuery(topicId))).Value;
        }
    }
}
