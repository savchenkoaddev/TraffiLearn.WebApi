using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Application.Services.Helpers;
using TraffiLearn.Application.Services.Mappers;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IQuestionsRepository _questionsRepository;
        private readonly QuestionsMapper _questionsMapper;

        public QuestionsService(IQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
            _questionsMapper = new QuestionsMapper();
        }

        public async Task AddAsync(QuestionAddRequest? item)
        {
            await ValidationHelper.ValidateObjects(item);

            await _questionsRepository.AddAsync(_questionsMapper.ToEntity(item));
        }

        public async Task DeleteAsync(QuestionDeleteRequest? request)
        {
            await ValidationHelper.ValidateObjects(request);

            var question = await GetByIdAsync(request!.Id!.Value);

            await _questionsRepository.DeleteAsync(request.Id.Value);
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllAsync()
        {
            return _questionsMapper.ToResponse(await _questionsRepository.GetAllAsync());
        }

        public async Task<QuestionResponse> GetByIdAsync(Guid? questionId)
        {
            await ValidationHelper.ValidateObjects(questionId);

            var question = await _questionsRepository.GetByIdAsync(questionId!.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(questionId.Value);
            }

            return _questionsMapper.ToResponse(question);
        }

        public async Task<QuestionResponse> GetRandomQuestionAsync()
        {
            var question = await _questionsRepository.GetRandomQuestionAsync();

            if (question is null)
            {
                throw new InvalidOperationException("Can't get random question because the storage is empty.");
            }

            return _questionsMapper.ToResponse(question);
        }

        public async Task UpdateAsync(Guid? questionId, QuestionUpdateRequest? item)
        {
            await ValidationHelper.ValidateObjects(questionId, item);

            var question = await GetByIdAsync(questionId);

            await _questionsRepository.UpdateAsync(questionId!.Value, _questionsMapper.ToEntity(item!));
        }
    }
}
