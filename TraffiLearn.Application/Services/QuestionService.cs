using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Application.Services.Helpers;
using TraffiLearn.Application.Services.Mappers;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private const int THEORY_TEST_QUESTIONS_QUANTITY = 20;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly QuestionMapper _questionsMapper;

        public QuestionService(IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _questionsMapper = new QuestionMapper();
        }

        #region Generic Methods


        //NOT EFFECTIVE
        //TO DO: OPTIMIZE
        public async Task AddAsync(QuestionAddRequest? request)
        {
            await ValidationHelper.ValidateObjects(request);

            var existingCategories = await _categoryRepository.GetAllAsync();
            var existingCategoryIds = existingCategories.Select(c => c.Id).ToHashSet();

            List<DrivingCategory> categoriesToAdd = new List<DrivingCategory>();

            foreach (var id in request.CategoriesIds)
            {
                if (!existingCategoryIds.Contains(id))
                {
                    throw new CategoryNotFoundException(id);
                }

                var drivingCategory = existingCategories.First(c => c.Id == id);
                categoriesToAdd.Add(drivingCategory);
            }

            var entity = _questionsMapper.ToEntity(request);
            entity.NumberDetails = request.NumberDetails;
            entity.DrivingCategories = categoriesToAdd;

            await _questionRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid? questionId)
        {
            await ValidationHelper.ValidateObjects(questionId);

            await ThrowIfQuestionNotFound(questionId.Value);
            await _questionRepository.DeleteAsync(questionId.Value);
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllAsync()
        {
            return _questionsMapper.ToResponse(await _questionRepository.GetAllAsync());
        }

        public async Task<QuestionResponse> GetByIdAsync(Guid? questionId)
        {
            await ValidationHelper.ValidateObjects(questionId);

            var question = await _questionRepository.GetByIdAsync(questionId!.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(questionId.Value);
            }

            return _questionsMapper.ToResponse(question);
        }

        public async Task UpdateAsync(Guid? questionId, QuestionUpdateRequest? request)
        {
            await ValidationHelper.ValidateObjects(questionId, request);

            await ThrowIfQuestionNotFound(questionId.Value);

            var question = await _questionRepository.GetByIdAsync(questionId.Value);
            var entity = _questionsMapper.ToEntity(request!);

            entity.NumberDetails = request.NumberDetails;
            entity.DrivingCategories = question.DrivingCategories;
            entity.Id = question.Id;

            await _questionRepository.UpdateAsync(questionId!.Value, entity);
        }


        #endregion

        public async Task<QuestionResponse> GetRandomQuestionAsync()
        {
            var question = await _questionRepository.GetRandomQuestion();

            if (question is null)
            {
                throw new InvalidOperationException("Can't get random question because the storage is empty.");
            }

            return _questionsMapper.ToResponse(question);
        }

        public async Task<IEnumerable<QuestionResponse>> GetQuestionsForCategory(Guid? categoryId)
        {
            await ValidationHelper.ValidateObjects(categoryId);
            await ThrowIfCategoryNotFound(categoryId.Value);

            var questions = await _questionRepository.GetQuestionsForCategory(categoryId.Value);

            return _questionsMapper.ToResponse(questions);
        }

        public async Task<QuestionResponse> GetRandomQuestionForCategory(Guid? categoryId)
        {
            await ValidationHelper.ValidateObjects(categoryId);
            await ThrowIfCategoryNotFound(categoryId.Value);

            var question = await _questionRepository.GetRandomQuestionForCategory(categoryId.Value);

            if (question is null)
            {
                throw new InvalidOperationException("Can't get random question because there are no questions for the category.");
            }

            return _questionsMapper.ToResponse(question);
        }

        public async Task<IEnumerable<QuestionResponse>> GetTheoryTestForCategory(Guid? categoryId)
        {
            await ValidationHelper.ValidateObjects(categoryId);

            //NOT EFFECTIVE
            //TO DO: OPTIMIZE
            var allQuestions = await _questionRepository.GetQuestionsForCategory(categoryId.Value);

            var random = new Random();
            var theoryTestQuestions = allQuestions
                .OrderBy(q => random.Next())
                .Take(THEORY_TEST_QUESTIONS_QUANTITY)
                .ToList();

            return _questionsMapper.ToResponse(theoryTestQuestions);
        }

        private async Task ThrowIfCategoryNotFound(Guid categoryId)
        {
            if (!await _categoryRepository.ExistsAsync(categoryId))
            {
                throw new CategoryNotFoundException(categoryId);
            }
        }

        private async Task ThrowIfQuestionNotFound(Guid questionId)
        {
            if (!await _questionRepository.ExistsAsync(questionId))
            {
                throw new QuestionNotFoundException(questionId);
            }
        }
    }
}
