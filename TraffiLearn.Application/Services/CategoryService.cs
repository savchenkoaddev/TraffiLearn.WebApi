using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.DTO.Categories.Response;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.ServiceContracts;
using TraffiLearn.Application.Services.Helpers;
using TraffiLearn.Application.Services.Mappers;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryMapper _categoryMapper = new();

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(CategoryRequest? request)
        {
            await ValidationHelper.ValidateObjects(request);

            var entity = _categoryMapper.ToEntity(request);
            
            await _categoryRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid? categoryId)
        {
            await ValidationHelper.ValidateObjects(categoryId);

            await ThrowIfCategoryNotFound(categoryId.Value);

            await _categoryRepository.DeleteAsync(categoryId!.Value);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            return _categoryMapper.ToResponse(await _categoryRepository.GetAllAsync());
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid? categoryId)
        {
            await ValidationHelper.ValidateObjects(categoryId);

            var found = await _categoryRepository.GetByIdAsync(categoryId.Value);

            if (found is null)
            {
                throw new CategoryNotFoundException(categoryId.Value);
            }

            return _categoryMapper.ToResponse(found);
        }

        public async Task UpdateAsync(Guid? categoryId, CategoryRequest? request)
        {
            await ValidationHelper.ValidateObjects(categoryId, request);

            await ThrowIfCategoryNotFound(categoryId.Value);

            var category = await _categoryRepository.GetByIdAsync(categoryId.Value);
            var entity = _categoryMapper.ToEntity(request!);

            entity.Questions = category.Questions;
            entity.Id = category.Id;

            await _categoryRepository.UpdateAsync(categoryId!.Value, entity);
        }

        private async Task ThrowIfCategoryNotFound(Guid categoryId)
        {
            if (!await _categoryRepository.ExistsAsync(categoryId))
            {
                throw new CategoryNotFoundException(categoryId);
            }
        }
    }
}
