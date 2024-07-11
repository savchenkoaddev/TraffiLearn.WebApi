﻿using TraffiLearn.Application.DTO.Categories.Request;
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

        public async Task AddAsync(CategoryRequest? item)
        {
            await ValidationHelper.ValidateObjects(item);

            var entity = _categoryMapper.ToEntity(item);
            
            await _categoryRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid? key)
        {
            await ValidationHelper.ValidateObjects(key);

            await ThrowIfCategoryNotFound(key.Value);

            await _categoryRepository.DeleteAsync(key!.Value);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            return _categoryMapper.ToResponse(await _categoryRepository.GetAllAsync());
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid? key)
        {
            await ValidationHelper.ValidateObjects(key);

            var found = await _categoryRepository.GetByIdAsync(key.Value);

            if (found is null)
            {
                throw new CategoryNotFoundException(key.Value);
            }

            return _categoryMapper.ToResponse(found);
        }

        public async Task UpdateAsync(Guid? key, CategoryRequest? item)
        {
            await ValidationHelper.ValidateObjects(key, item);

            var question = await GetByIdAsync(key);
            var entity = _categoryMapper.ToEntity(item!);

            entity.Id = question.Id;

            await _categoryRepository.UpdateAsync(key!.Value, entity);
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
