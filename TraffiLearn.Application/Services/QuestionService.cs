using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Application.Services.Helpers;
using TraffiLearn.Application.Services.Mappers;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly QuestionMapper _questionsMapper;

        public QuestionService(IQuestionRepository questionsRepository, ICategoryRepository categoryRepository)
        {
            _questionsRepository = questionsRepository;
            _categoryRepository = categoryRepository;
            _questionsMapper = new QuestionMapper();
        }

        #region Generic Methods


        public async Task AddAsync(QuestionAddRequest? request)
        {
            await ValidationHelper.ValidateObjects(request);
            var entity = _questionsMapper.ToEntity(request);

            entity.NumberDetails = request.NumberDetails;

            await _questionsRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid? questionId)
        {
            await ValidationHelper.ValidateObjects(questionId);

            await ThrowIfQuestionNotFound(questionId.Value);
            await _questionsRepository.DeleteAsync(questionId.Value);
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

        public async Task UpdateAsync(Guid? questionId, QuestionUpdateRequest? request)
        {
            await ValidationHelper.ValidateObjects(questionId, request);

            var question = await GetByIdAsync(questionId);
            var entity = _questionsMapper.ToEntity(request!);

            entity.NumberDetails = request.NumberDetails;
            entity.Id = question.Id;

            await _questionsRepository.UpdateAsync(questionId!.Value, entity);
        }


        #endregion

        public async Task<QuestionResponse> GetRandomQuestionAsync()
        {
            var question = await _questionsRepository.GetRandomQuestionAsync();

            if (question is null)
            {
                throw new InvalidOperationException("Can't get random question because the storage is empty.");
            }

            return _questionsMapper.ToResponse(question);
        }

        private async Task ThrowIfQuestionNotFound(Guid questionId)
        {
            if (!await _questionsRepository.ExistsAsync(questionId))
            {
                throw new QuestionNotFoundException(questionId);
            }
        }
    }
}
