using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository _questionsRepository;

        public QuestionsService(IQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public Task AddAsync(Question item)
        {

        }

        public Task DeleteAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetByIdAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetRandomQuestionAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid questionId, Question item)
        {
            throw new NotImplementedException();
        }
    }
}
